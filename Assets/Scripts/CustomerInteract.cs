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
        if (!other.CompareTag("Player")) return;
        pressKeyUI.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        pressKeyUI.SetActive(false);
    }

    void Update()
    {

        if (!pressKeyUI.activeSelf)
            return;

        if (!QueueManager.Instance.IsFrontCustomer(customer))
            return;

        if (customer.HasOrdered)   // ⭐ 추가
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            OrderManager.Instance.StartOrder(customer);
            pressKeyUI.SetActive(false);
        }


    }

   
}
