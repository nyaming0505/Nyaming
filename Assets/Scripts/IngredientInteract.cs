using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientInteract : MonoBehaviour
{

    public GameObject pressKeyUI;
    public GameObject miniGameUI;

    bool playerInRange = false;
    bool inputLocked = false;   // ? 입력 잠금

    PlayerInteraction player;

    void Start()
    {
        pressKeyUI.SetActive(false);
        miniGameUI.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange || player == null || inputLocked)
            return;

        if (MiniGameState.isBusy)
            return;

        if (MiniGameState.waitForKeyRelease)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                MiniGameState.waitForKeyRelease = false;
            }
            return;
        }


        if (player.isInteracting)
        {
            StartMiniGame();
        }
    }


    void StartMiniGame()
    {
        MiniGameState.isBusy = true; // ?? 전체 잠금

        miniGameUI.SetActive(true);
        pressKeyUI.SetActive(false);
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
            inputLocked = false;

            if (MiniGameState.isBusy)
            {
                MiniGameState.isBusy = false;
                MiniGameState.waitForKeyRelease = false;
            }

            miniGameUI.SetActive(false);
            pressKeyUI.SetActive(false);
        }
    }

}
