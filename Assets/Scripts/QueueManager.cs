using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public Transform[] queuePoints; // size = 3
    private Customer[] slots = new Customer[3];

    public bool TryEnterQueue(Customer customer)
    {
        for (int i = slots.Length - 1; i >= 0; i--)
        {
            if (slots[i] == null)
            {
                slots[i] = customer;
               // customer.MoveTo(queuePoints[i].position);
                return true;
            }
        }
        return false; // ¡Ÿ ∞°µÊ
    }

    public void AdvanceQueue()
    {
        for (int i = 0; i < slots.Length - 1; i++)
        {
            slots[i] = slots[i + 1];
           // if (slots[i] != null)
             //   slots[i].MoveTo(queuePoints[i].position);
        }
        slots[slots.Length - 1] = null;
    }
}
