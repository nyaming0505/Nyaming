using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Progress")]
    public int successCustomerCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // =========================
    // 성공 처리
    // =========================
    public void OnCustomerServed()
    {
        successCustomerCount++;
        Debug.Log($"[LEVEL] 성공 손님 수: {successCustomerCount}");
    }

    // =========================
    // 현재 레벨
    // =========================
    public int GetCurrentLevel()
    {
        if (successCustomerCount >= 30) return 7;
        if (successCustomerCount >= 25) return 6;
        if (successCustomerCount >= 20) return 5;
        if (successCustomerCount >= 15) return 4;
        if (successCustomerCount >= 10) return 3;
        if (successCustomerCount >= 5) return 2;
        return 1;
    }

    // =========================
    // 최대 동시 손님 수
    // =========================
    public int GetMaxTotalCustomers()
    {
        switch (GetCurrentLevel())
        {
            case 1: return 1;
            case 2: return 2;
            case 3: return 3;
            case 4: return 4;
            case 5: return 5;
            case 6: return 6;
            case 7: return 7;
            default: return 1;
        }
    }
}
