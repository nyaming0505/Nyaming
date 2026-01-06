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
        if (QueueManager.Instance.IsFrontCustomer(customer))
        {
            pressKeyUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        pressKeyUI.SetActive(false);
    }

    void Update()
    {

        if (pressKeyUI.activeSelf &&
             !QueueManager.Instance.IsFrontCustomer(customer))
        {
            pressKeyUI.SetActive(false);
            return;
        }

        if (!pressKeyUI.activeSelf)
            return;

        // =========================
        // 주문 받기
        // =========================
        if (!customer.HasOrdered)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OrderManager.Instance.StartOrder(customer);
                pressKeyUI.SetActive(false);
            }
        }

        // =========================
        // 음료 서빙
        // =========================
        else
        {
            if (customer.currentState != CustomerState.WaitingForDrink)
                return;

            if (CupManager.Instance.GetIngredientCount() == 0)
                return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                OrderManager.Instance.CheckOrder(customer);
          
                pressKeyUI.SetActive(false);
            }
        }

    }
   
}
