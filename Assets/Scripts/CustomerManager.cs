using System.Collections;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    [Header("Spawn Settings")]
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 1f;

    int currentTotalCustomer;

    Coroutine spawnCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartSpawning();
    }

    void StartSpawning()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            TrySpawnCustomer();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void TrySpawnCustomer()
    {
        int maxAllowed = LevelManager.Instance.GetMaxTotalCustomers();

        if (currentTotalCustomer >= maxAllowed)
            return;

        SpawnCustomer();
    }

    void SpawnCustomer()
    {
        Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        currentTotalCustomer++;
    }

    // ? 손님이 성공/실패로 나갈 때 반드시 호출
    public void OnCustomerLeave()
    {
        currentTotalCustomer--;
        currentTotalCustomer = Mathf.Max(0, currentTotalCustomer);
    }

    // 디버깅용
    public int GetCurrentCustomerCount()
    {
        return currentTotalCustomer;
    }
}
