using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanInteract : MonoBehaviour
{
    public GameObject pressKeyUI;

    bool playerInRange = false;
    bool inputLocked = false;

    PlayerInteraction player;

    void Start()
    {
        pressKeyUI.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange || player == null || inputLocked)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            ThrowAway();
        }
    }

    void ThrowAway()
    {
        inputLocked = true;

        CupManager.Instance.ClearCup();
        pressKeyUI.SetActive(false);

        // 입력 너무 연속되는 것 방지
        Invoke(nameof(UnlockInput), 0.2f);
    }

    void UnlockInput()
    {
        inputLocked = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.GetComponent<PlayerInteraction>();
            pressKeyUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            pressKeyUI.SetActive(false);
            inputLocked = false;
        }
    }
}
