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
    Enter,          // 입장
    MoveToOrder,    // 주문대로 이동
    Order,          // 주문 제시
    MoveToSeat,     // 자리로 이동
    Waiting,        // 기다림 (타이머)
    Success,        // 메뉴 받음
    Fail,           // 못 받음
    Leave           // 퇴장
}


public class Customer : MonoBehaviour
{
    [Header("State")]
    public CustomerState currentState;

    [Header("Order")]
    public List<CupIngredient> orderRecipe;

    [Header("Wait")]
    public float maxWaitTime = 20f;
    private float waitTimer;

    [Header("Movement")]
    public Transform orderPoint;
    public Transform seatPoint;
    public Transform exitPoint;
    public float moveSpeed = 2f;

    [Header("References")]
    public Animator anim;

    private bool isServed = false;

    void Start()
    {
        ChangeState(CustomerState.Enter);
    }

    void Update()
    {
        UpdateState();
    }
    void UpdateState()
    {
        switch (currentState)
        {
            case CustomerState.Enter:
                MoveTo(orderPoint, CustomerState.Order);
                break;

            case CustomerState.MoveToSeat:
                MoveTo(seatPoint, CustomerState.Waiting);
                break;

            case CustomerState.Waiting:
                UpdateWaiting();
                break;

            case CustomerState.Leave:
                MoveTo(exitPoint, null);
                break;
        }
    }
    void MoveTo(Transform target, CustomerState? nextState)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            if (nextState.HasValue)
                ChangeState(nextState.Value);
        }
    }

    void UpdateWaiting()
    {
        if (isServed) return;

        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0f)
        {
            FailOrder();
        }
    }
    public void TryServe()
    {
        if (currentState != CustomerState.Waiting) return;

        if (IsOrderCorrect())
        {
            SuccessOrder();
        }
        else
        {
            FailOrder();
        }
    }

    bool IsOrderCorrect()
    {
        List<CupIngredient> cup = CupManager.Instance.GetIngredients();

        if (cup.Count != orderRecipe.Count) return false;

        List<CupIngredient> tempCup = new List<CupIngredient>(cup);

        foreach (var ing in orderRecipe)
        {
            if (!tempCup.Remove(ing))
                return false;
        }

        return true;
    }
    void SuccessOrder()
    {
        isServed = true;

        GameManager.Instance.AddScore(100);
        CupManager.Instance.ClearCup();

        ChangeState(CustomerState.Success);
        Invoke(nameof(LeaveCustomer), 1.5f);
    }

    void FailOrder()
    {
        isServed = true;

      //  GameManager.Instance.DecreaseHeart(); // 
        CupManager.Instance.ClearCup();

        ChangeState(CustomerState.Fail);
        Invoke(nameof(LeaveCustomer), 1.5f);
    }

    void LeaveCustomer()
    {
        ChangeState(CustomerState.Leave);
    }
    void ChangeState(CustomerState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case CustomerState.Enter:
                anim.Play("Walk");
                break;

            case CustomerState.Order:
                anim.Play("Idle");
           //     ShowOrderUI();
                break;

            case CustomerState.Waiting:
                waitTimer = maxWaitTime;
                anim.Play("Sit");
                break;

            case CustomerState.Success:
                anim.Play("Happy");
                break;

            case CustomerState.Fail:
                anim.Play("Angry");
                break;

            case CustomerState.Leave:
                anim.Play("Walk");
                break;
        }
    }

}
