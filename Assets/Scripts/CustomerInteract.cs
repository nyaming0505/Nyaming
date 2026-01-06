using UnityEngine;

public class CustomerInteract : MonoBehaviour
{
    public GameObject pressKeyUI;

    Customer customer;
    bool isPlayerInRange = false;

    void Start()
    {
        customer = GetComponent<Customer>();
        pressKeyUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        isPlayerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        isPlayerInRange = false;
        pressKeyUI.SetActive(false);
    }

    void Update()
    {
        if (!isPlayerInRange)
            return;

        // =========================
        // 주문 받기 (줄 서있는 손님)
        // =========================
        if (!customer.HasOrdered)
        {
            bool canOrder =
                customer.currentState == CustomerState.WaitingInQueue &&
                QueueManager.Instance.IsFrontCustomer(customer);

            pressKeyUI.SetActive(canOrder);

            if (!canOrder)
                return;

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
            bool canServe =
                customer.currentState == CustomerState.WaitingForDrink &&
                CupManager.Instance.GetIngredientCount() > 0;

            pressKeyUI.SetActive(canServe);

            if (!canServe)
                return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                OrderManager.Instance.CheckOrder(customer);
                pressKeyUI.SetActive(false);
            }
        }
    }
}
