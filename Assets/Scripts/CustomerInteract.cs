using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteract : MonoBehaviour
{
    public GameObject pressKeyUI;
    Customer customer;

    void Start()
    {
        customer = GetComponent<Customer>();
        pressKeyUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            pressKeyUI.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            pressKeyUI.SetActive(false);
    }

    void Update()
    {
        if (!pressKeyUI.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            //
        }
    }
}

