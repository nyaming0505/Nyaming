using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class OrderData
{
    public Customer customer;
    public Recipe recipe;

    public OrderData(Customer customer, Recipe recipe)
    {
        this.customer = customer;
        this.recipe = recipe;
    }
}


public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [Header("References")]
    public RecipeDatabase recipeDatabase;

    [Header("Settings")]
    public int maxOrders = 4;

    private List<OrderData> activeOrders = new List<OrderData>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // =========================
    // 주문 시작
    // =========================
    public void StartOrder(Customer customer)
    {
        if (customer == null)
            return;

        if (customer.HasOrdered)
        {
            Debug.Log("이미 주문한 손님입니다.");
            return;
        }

        if (activeOrders.Count >= maxOrders)
        {
            Debug.Log("최대 주문 수 도달");
            return;
        }

        Chair chair = ChairManager.Instance.GetEmptyChair();
        if (chair == null)
        {
            Debug.Log("의자가 없어서 주문을 받을 수 없습니다!");
            return;
        }

        Recipe recipe = recipeDatabase.GetRandomRecipe();

        OrderData order = new OrderData(customer, recipe);
        activeOrders.Add(order);

        Debug.Log($"[ORDER] 추가됨: {recipe.recipeName} / 현재 주문 수: {activeOrders.Count}");
        chair.TryOccupy();
        customer.MarkOrdered();
        customer.OnOrderTaken(chair);

        // TODO : 주문 UI 추가
    }

    // =========================
    // 주문 판정 (컵 기준)
    // =========================
    public void CheckOrder()
    {
        if (activeOrders.Count == 0)
        {
            Debug.Log("처리할 주문 없음");
            return;
        }

        // ⭐ 일단 제일 먼저 받은 주문부터 처리
        OrderData order = activeOrders[0];

        bool success = RecipeChecker.Check(
            CupManager.Instance.GetIngredients(),
            order.recipe
        );

        if (success)
        {
            Debug.Log("[ORDER] 성공");
            order.customer.OnDrinkServed();
            ScoreManager.Instance.AddScore(100);
        }
        else
        {
            Debug.Log("[ORDER] 실패");
            order.customer.OnTimeOver();
        }

        activeOrders.RemoveAt(0);
        CupManager.Instance.ClearCup();

        // TODO : 주문 UI 갱신
    }

    public int GetOrderCount()
    {
        return activeOrders.Count;
    }

#if UNITY_EDITOR
    void Update()
    {
        // 테스트용
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckOrder();
        }
    }
#endif
}
