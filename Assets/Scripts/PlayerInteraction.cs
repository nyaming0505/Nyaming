using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public bool isInteracting { get; private set; }

    void Update()
    {
        isInteracting = Input.GetKey(KeyCode.E);
    }
}
