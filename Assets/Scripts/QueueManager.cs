using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public static QueueManager Instance;

    [Header("Queue Points")]
    public Transform[] waitPoints;

    private List<Customer> queue = new List<Customer>();

    void Awake()
    {
        Instance = this;
    }

    // =========================
    // 큐 진입 시도
    // =========================
    public bool TryEnterQueue(Customer customer)
    {
        if (queue.Count >= waitPoints.Length)
            return false;

        queue.Add(customer);
        UpdateQueuePositions();
        return true;
    }

    // =========================
    // 큐에서 나가기
    // =========================
    public void LeaveQueue(Customer customer)
    {
        if (queue.Remove(customer))
        {
            UpdateQueuePositions();
        }
    }

    // =========================
    // 큐 재정렬
    // =========================
    void UpdateQueuePositions()
    {
        for (int i = 0; i < queue.Count; i++)
        {
            queue[i].MoveTo(waitPoints[i].position);
        }
    }

    // =========================
    // 제일 앞 손님
    // =========================
    public Customer GetFrontCustomer()
    {
        if (queue.Count == 0) return null;
        return queue[0];
    }

    public int GetQueueCount()
    {
        return queue.Count;
    }

    public bool IsFrontCustomer(Customer customer)
    {
        if (queue.Count == 0) return false;
        return queue[0] == customer;
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (waitPoints == null) return;

        Gizmos.color = Color.yellow;
        foreach (var point in waitPoints)
        {
            if (point != null)
                Gizmos.DrawSphere(point.position, 0.2f);
        }
    }
#endif
}

