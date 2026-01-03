using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    LeaveFail
}

public class Customer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("References")]
    public Animator animator;

    public CustomerState currentState;

    Vector2 targetPosition;
    bool isMoving = false;

    Chair assignedChair;

    public bool HasOrdered { get; private set; }


    // =========================
    // Unity Life Cycle
    // =========================

    void Start()
    {
        ChangeState(CustomerState.Enter);
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
                StopMoving();
                break;

            case CustomerState.Ordering:
                StopMoving();
                // 주문 생성은 외부(OrderManager)에서 처리
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
                StopMoving();
                break;

            case CustomerState.LeaveSuccess:
            case CustomerState.LeaveFail:

                if (assignedChair != null)
                {
                    assignedChair.Release();
                    assignedChair = null;
                }
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

            case CustomerState.MoveToSeat:
                if (!isMoving && assignedChair != null)
                {
                    SitOnChair(assignedChair);
                    ChangeState(CustomerState.WaitingForDrink);
                }
                break;
            case CustomerState.LeaveSuccess:
            case CustomerState.LeaveFail:
                Destroy(gameObject);
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

        // ⭐ X축 먼저 이동
        if (Mathf.Abs(delta.x) > 0.01f)
        {
            next.x = Mathf.MoveTowards(
                current.x,
                targetPosition.x,
                moveSpeed * Time.deltaTime
            );

            UpdateAnimation(new Vector2(delta.x, 0));
        }
        // ⭐ X가 맞으면 Y축 이동
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
            // 도착
            transform.position = targetPosition;
            isMoving = false;
            animator.SetBool("IsMoving", false);
            return;
        }

        transform.position = next;
    }


    public void MoveTo(Vector2 pos)
    {
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
        }
        else
        {
            moveX = 0;
            moveY = Mathf.Sign(dir.y);
        }

        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);

        // ⭐ 마지막 방향 저장
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

    public void OnOrderTaken(Chair chair)
    {
        QueueManager.Instance.LeaveQueue(this); // ⭐ 핵심
        assignedChair = chair;
        ChangeState(CustomerState.MoveToSeat);
    }

    public void OnDrinkServed()
    {
        ChangeState(CustomerState.LeaveSuccess);
    }

    public void OnTimeOver()
    {
        ChangeState(CustomerState.LeaveFail);
    }

    public void MarkOrdered()
    {
        HasOrdered = true;
    }

    public void SitOnChair(Chair chair)
    {
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsSitting", true);

        if (chair.sitDirection == SitDirection.Left)
        {
            animator.SetFloat("lastMoveX", -1);
            animator.SetFloat("lastMoveY", 0);
        }
        else
        {
            animator.SetFloat("lastMoveX", 1);
            animator.SetFloat("lastMoveY", 0);
        }
    }
}