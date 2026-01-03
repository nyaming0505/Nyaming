using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    [Header("Spawn Settings")]
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 1f;

    [Header("Limits")]
    public int maxQueueCount = 3;
    public int maxTotalCustomer = 7; // 큐 + 의자

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(SpawnRoutine());
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
        int queueCount = QueueManager.Instance.GetQueueCount();
        int seatCount = ChairManager.Instance.GetOccupiedCount();
        int total = queueCount + seatCount;

        // ?? 조건 체크
        if (queueCount >= maxQueueCount)
            return;

        if (total >= maxTotalCustomer)
            return;

        Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("손님 생성됨");
    }
}
