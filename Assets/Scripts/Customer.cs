using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class RecipeChecker
{

    //레시피 체크
    public static bool Check(List<CupIngredient> cup, Recipe recipe)
    {
        if (cup.Count != recipe.ingredients.Count)
            return false;

        List<CupIngredient> cupCopy = new List<CupIngredient>(cup);
        List<CupIngredient> recipeCopy = new List<CupIngredient>(recipe.ingredients);

        foreach (var ingredient in recipeCopy)
        {
            if (!cupCopy.Contains(ingredient))
                return false;

            cupCopy.Remove(ingredient);
        }

        return true;
    }
}

public enum CustomerState
{
    Enter,
    MoveToQueue,
    WaitingInQueue,
    Ordering,
    MoveToSeat,
    WaitingForDrink,
    LeaveSuccess,
    LeaveFail,
    LeaveToPath,  
    LeaveExit,
}

public class Customer : MonoBehaviour
{
    public CustomerBubbleUI bubbleUI;

    [Header("Queue Waiting")]
    public float maxQueueWaitTime = 8f;
    float currentQueueWaitTime;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("References")]
    public Animator animator;

    public CustomerState currentState;

    Vector2 targetPosition;
    bool isMoving = false;

    Chair assignedChair;
    bool hasLeft = false;

    public bool HasOrdered { get; private set; }

    bool exitMoveFirstY = false;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        ChangeState(CustomerState.Enter);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Move();
        UpdateState();
    }

    // =========================
    // 상태 변경
    // =========================

    void ChangeState(CustomerState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case CustomerState.Enter:
                MoveTo(PathManager.Instance.GetEnterPoint());
                break;

            case CustomerState.MoveToQueue:
                bool success = QueueManager.Instance.TryEnterQueue(this);
                if (!success)
                {
                    ChangeState(CustomerState.LeaveFail);
                    return;
                }
                break;


            case CustomerState.WaitingInQueue:
                currentQueueWaitTime = maxQueueWaitTime;
                bubbleUI.ShowWaitGauge();
                StopMoving();
                break;

            case CustomerState.Ordering:
                StopMoving();
                break;

            case CustomerState.MoveToSeat:

                if (assignedChair == null)
                {
                    ChangeState(CustomerState.LeaveFail);
                    return;
                }
                MoveTo(assignedChair.sitPoint.position);
                break;

            case CustomerState.WaitingForDrink:
                bubbleUI.ShowWaitGauge();
                StopMoving();
                break;

            case CustomerState.LeaveSuccess:
            case CustomerState.LeaveFail:
                QueueManager.Instance.LeaveQueue(this);
                animator.SetBool("IsSitting", false);

                if (assignedChair != null)
                {
                    assignedChair.Release();
                    assignedChair = null;
                }

                ChangeState(CustomerState.LeaveToPath);
                break;

            case CustomerState.LeaveToPath:
                exitMoveFirstY = true;
                MoveTo(PathManager.Instance.GetExitMidPoint());
                break;

            case CustomerState.LeaveExit:
                MoveTo(PathManager.Instance.GetExitPoint());
                break;

        }
    }

    // =========================
    // 상태 업데이트
    // =========================

    void UpdateState()
    {
        if (isMoving) return;

        switch (currentState)
        {
            case CustomerState.Enter:
                ChangeState(CustomerState.MoveToQueue);
                break;

            case CustomerState.MoveToQueue:
                ChangeState(CustomerState.WaitingInQueue);
                break;

            case CustomerState.WaitingInQueue:
                currentQueueWaitTime -= Time.deltaTime;

                float ratio = currentQueueWaitTime / maxQueueWaitTime;
                bubbleUI.SetWaitGauge(ratio);

                if (currentQueueWaitTime <= 0f)
                {
                    bubbleUI.HideWaitGauge();
                    OnTimeOver();
                }
                break;

            case CustomerState.MoveToSeat:
                if (!isMoving && assignedChair != null)
                {
                    SitOnChair(assignedChair);
                    ChangeState(CustomerState.WaitingForDrink);
                }
                break;

            case CustomerState.LeaveToPath:
                ChangeState(CustomerState.LeaveExit);
                break;

            case CustomerState.LeaveExit:
                Leave();
                break;
            case CustomerState.LeaveSuccess:
            case CustomerState.LeaveFail:
                Leave();
                break;
        }
    }

    // =========================
    // 이동 로직 (4방향)
    // =========================

    void Move()
    {
        if (!isMoving)
        {
            animator.SetBool("IsMoving", false);
            return;
        }

        Vector2 current = transform.position;
        Vector2 next = current;
        Vector2 delta = targetPosition - current;

        if (exitMoveFirstY)
        {
            // ⭐ Y → X 이동
            if (Mathf.Abs(delta.y) > 0.01f)
            {
                next.y = Mathf.MoveTowards(
                    current.y,
                    targetPosition.y,
                    moveSpeed * Time.deltaTime
                );

                UpdateAnimation(new Vector2(0, delta.y));
            }
            else if (Mathf.Abs(delta.x) > 0.01f)
            {
                next.x = Mathf.MoveTowards(
                    current.x,
                    targetPosition.x,
                    moveSpeed * Time.deltaTime
                );

                UpdateAnimation(new Vector2(delta.x, 0));
            }
            else
            {
                Arrive();
            }
        }
        else
        {
            // ⭐ 기존 X → Y 이동
            if (Mathf.Abs(delta.x) > 0.01f)
            {
                next.x = Mathf.MoveTowards(
                    current.x,
                    targetPosition.x,
                    moveSpeed * Time.deltaTime
                );

                UpdateAnimation(new Vector2(delta.x, 0));
            }
            else if (Mathf.Abs(delta.y) > 0.01f)
            {
                next.y = Mathf.MoveTowards(
                    current.y,
                    targetPosition.y,
                    moveSpeed * Time.deltaTime
                );

                UpdateAnimation(new Vector2(0, delta.y));
            }
            else
            {
                Arrive();
            }
        }

        transform.position = next;
    }

    void Arrive()
    {
        transform.position = targetPosition;
        isMoving = false;
        exitMoveFirstY = false; // ⭐ 리셋 중요
        animator.SetBool("IsMoving", false);
    }


    public void MoveTo(Vector2 pos)
    {
        animator.SetBool("IsSitting", false);
        targetPosition = pos;
        isMoving = true;
    }

    void StopMoving()
    {
        isMoving = false;
        animator.SetBool("IsMoving", false);
    }

    // =========================
    // 애니메이션 (상 / 하 / 좌 / 우)
    // =========================

    void UpdateAnimation(Vector2 dir)
    {
        animator.SetBool("IsMoving", true);

        float moveX = 0;
        float moveY = 0;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            moveX = Mathf.Sign(dir.x);
            moveY = 0;

            // ⭐ 좌우 이동일 때만 flip 처리
            if (dir.x < 0)
                spriteRenderer.flipX = true;   // 왼쪽
            else if (dir.x > 0)
                spriteRenderer.flipX = false;  // 오른쪽
        }
        else
        {
            moveX = 0;
            moveY = Mathf.Sign(dir.y);
            // 상하 이동 시 flip 유지 (변경 안 함)
        }

        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);

        animator.SetFloat("lastMoveX", moveX);
        animator.SetFloat("lastMoveY", moveY);
    }


    // =========================
    // 외부 이벤트 호출
    // =========================

    public void OnOrderCalled()
    {
        ChangeState(CustomerState.Ordering);
    }

    public void OnOrderTaken(Chair chair, Recipe recipe)
    {
        QueueManager.Instance.LeaveQueue(this); // ⭐ 핵심
        assignedChair = chair;
        bubbleUI.ShowOrder(recipe.orderSprite);
        ChangeState(CustomerState.MoveToSeat);
    }

    public void OnDrinkServed()
    {
        bubbleUI.HideWaitGauge();
        bubbleUI.ShowResult(true);
        ChangeState(CustomerState.LeaveSuccess);
    }

    public void OnTimeOver()
    {

        bubbleUI.HideWaitGauge();
        bubbleUI.ShowResult(false);
        ChangeState(CustomerState.LeaveFail);
        if (HeartManager.instance != null)
        {
            HeartManager.instance.OnOrderFailed();
        }
    }

    public void MarkOrdered()
    {
        HasOrdered = true;
    }

    public void SitOnChair(Chair chair)
    {
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsSitting", true);

        // 방향 값은 애니메이터용 (선택)
        animator.SetFloat("lastMoveY", 0);

        if (chair.sitDirection == SitDirection.Left)
        {
            animator.SetFloat("lastMoveX", -1);
            spriteRenderer.flipX = true;
        }
        else
        {
            animator.SetFloat("lastMoveX", 1);
            spriteRenderer.flipX = false;
        }
    }

    public void UpdateServiceGauge(float ratio)
    {
        bubbleUI.ShowWaitGauge();
        bubbleUI.SetWaitGauge(ratio);
    }
    public void HideServiceGauge()
    {
        bubbleUI.HideWaitGauge();
    }

    void Leave()
    {
        if (hasLeft) return;
        hasLeft = true;

        CustomerManager.Instance.OnCustomerLeave();
        Destroy(gameObject);
    }
}