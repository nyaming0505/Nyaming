using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int maxSafeCustomer = 1;

    void Awake()
    {
        Instance = this;
    }

    public float GetQueueWaitMultiplier(int queueCount)
    {
        if (queueCount <= maxSafeCustomer)
            return 1f;

        return 1f + (queueCount - maxSafeCustomer) * 0.5f;
    }
}
