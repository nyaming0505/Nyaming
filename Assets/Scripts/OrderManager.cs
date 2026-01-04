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

    [Header("Order UI")]
    public Transform orderUIParent;
    public List<OrderUIPrefabEntry> orderUIPrefabs;

    Dictionary<RecipeType, GameObject> prefabMap;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        prefabMap = new Dictionary<RecipeType, GameObject>();
        foreach (var entry in orderUIPrefabs)
            prefabMap[entry.recipeType] = entry.orderUIPrefab;
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


        //오더UI 생성
        if (!prefabMap.TryGetValue(recipe.recipeType, out var prefab))
        {
            Debug.LogError($"프리팹 없음: {recipe.recipeType}");
            return;
        }

        GameObject ui = Instantiate(prefab, orderUIParent);
        ui.GetComponent<OrderUIItem>().Init(customer, recipe);


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
            Debug.Log("해당 손님의 주문이 없습니다.");
            return;
        }
        bool success = RecipeChecker.Check(
        CupManager.Instance.GetIngredients(),
        order.recipe
    );
        if (success)
        {
            Debug.Log("[ORDER] 성공");
            targetCustomer.OnDrinkServed();
          //  ScoreManager.Instance.AddScore(order.recipe.score);
        }
        else
        {
            Debug.Log("[ORDER] 실패");
            targetCustomer.OnTimeOver();
        }

        activeOrders.Remove(order);
        CupManager.Instance.ClearCup();

        // TODO : 해당 손님의 주문 UI 제거

    }

    public int GetOrderCount()
    {
        return activeOrders.Count;
    }
}
