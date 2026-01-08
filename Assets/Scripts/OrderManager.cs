using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class OrderData
{
    public Customer customer;
    public Recipe recipe;
    public OrderUIItem uiItem;
    public float remainTime;
    public float maxTime;

    public OrderData(Customer customer, Recipe recipe, OrderUIItem uiItem, float time)
    {
        this.customer = customer;
        this.recipe = recipe;
        this.uiItem = uiItem;
        this.maxTime = time;
        this.remainTime = time;
    }

}


public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;
    [Header("Order Time")]
    public float waitForDrinkTime = 20f;

    [Header("References")]
    public RecipeDatabase recipeDatabase;

    [Header("Settings")]
    public int maxOrders = 4;

    private List<OrderData> activeOrders = new List<OrderData>();

    [Header("Order UI")]
    public Transform orderUIParent;
    public List<OrderUIPrefabEntry> orderUIPrefabs;

    Dictionary<RecipeType, GameObject> prefabMap;

    public float timeMultiplier = 1.0f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        timeMultiplier = 1.0f;

        prefabMap = new Dictionary<RecipeType, GameObject>();
        foreach (var entry in orderUIPrefabs)
            prefabMap[entry.recipeType] = entry.orderUIPrefab;
    }

    void Update()
    {
        UpdateOrderTimers();
    }
    void UpdateOrderTimers()
    {
        for (int i = activeOrders.Count - 1; i >= 0; i--)
        {
            OrderData order = activeOrders[i];

            order.remainTime -= Time.deltaTime;

            float ratio = order.remainTime / order.maxTime;

            // 주문 UI 갱신
            if (order.uiItem != null)
                order.uiItem.UpdateTimer(ratio);

            // 🔥 손님 머리 위 게이지 갱신
            if (order.customer != null)
                order.customer.UpdateServiceGauge(ratio);


            // ⏰ 시간 초과
            if (order.remainTime <= 0f)
            {
                HandleOrderTimeOver(order);
            }
        }
    }

    void HandleOrderTimeOver(OrderData order)
    {
        order.customer.OnTimeOver();

        if (order.uiItem != null)
            Destroy(order.uiItem.gameObject);

        activeOrders.Remove(order);
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
            return;
        }

        if (activeOrders.Count >= maxOrders)
        {
            return;
        }

        Chair chair = ChairManager.Instance.GetEmptyChair();
        if (chair == null)
        {
            return;
        }

        Recipe recipe = recipeDatabase.GetRandomRecipe();
        chair.TryOccupy();
        customer.MarkOrdered();
        customer.OnOrderTaken(chair, recipe);


        //오더UI 생성
        if (!prefabMap.TryGetValue(recipe.recipeType, out var prefab))
        {
            return;
        }

        GameObject uiObj = Instantiate(prefab, orderUIParent);
        OrderUIItem uiItem = uiObj.GetComponent<OrderUIItem>();
        uiItem.Init(customer, recipe);

        float baseTime = LevelManager.Instance.GetWaitForDrinkTime();
        float finalTime = baseTime * timeMultiplier;

        OrderData order = new OrderData(
            customer,
            recipe,
            uiItem,
            finalTime
        );

        activeOrders.Add(order);

    }

    // =========================
    // 주문 판정 (컵 기준)
    // =========================
    public void CheckOrder(Customer targetCustomer)
    {
        if (targetCustomer == null)
            return;

        // 해당 손님의 주문 찾기
        OrderData order = activeOrders.Find(o => o.customer == targetCustomer);

        if (order == null)
        {
            return;
        }
        bool success = RecipeChecker.Check(
        CupManager.Instance.GetIngredients(),
        order.recipe
    );
        if (success)
        {
            targetCustomer.OnDrinkServed();
            ScoreManager.Instance.AddScore(order.recipe.score);

            LevelManager.Instance.OnCustomerServed();
        }
        else
        {
            targetCustomer.OnTimeOver();
        }


        // TODO : 해당 손님의 주문 UI 제거
        if (order.uiItem != null)
        {
            Destroy(order.uiItem.gameObject);
        }

        activeOrders.Remove(order);
        CupManager.Instance.ClearCup();

    }

    public int GetOrderCount()
    {
        return activeOrders.Count;
    }

    public void ActivateDoubleTimeBug()
    {
        timeMultiplier = 2.0f;

        foreach (var order in activeOrders)
        {
            order.remainTime *= 2f;
            order.maxTime *= 2f;
        }
    }
}
