    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using static SoundManager;

public class CoffeeMachineInteract : MonoBehaviour
{
    public GameObject progressBarAll;
    public Image progressBar;
    public GameObject pressKeyUI;
    public float fillSpeed = 0.7f;

    public Tilemap tilemap;
    public TileBase idleTile;
    public TileBase changedTile;

    public Vector3Int machineCellPos;

    float progress = 0f;
    bool playerInRange = false;
    PlayerInteraction player;

    void Start()
    {
        progressBar.fillAmount = 0f;
        pressKeyUI.SetActive(false);
        progressBarAll.SetActive(false);

        tilemap.SetTile(machineCellPos, idleTile);
    }

    void Update()
    {
        if (!playerInRange || player == null)
            return;

        if (player.isInteracting)
        {
            progress += fillSpeed * Time.deltaTime;
            progressBar.fillAmount = progress;

            tilemap.SetTile(machineCellPos, changedTile);

            if (progress >= 1f)
            {
                CompleteCoffee();
                tilemap.SetTile(machineCellPos, idleTile);
            }
        }
    }

    void CompleteCoffee()
    {
        progress = 0f;
        progressBar.fillAmount = 0f;
        SoundManager.Instance.PlaySFX(SFXType.CoffeeShot);
        CupManager.Instance.AddIngredient(CupIngredient.Espresso);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.GetComponent<PlayerInteraction>();
            pressKeyUI.SetActive(true);
            progressBarAll.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            pressKeyUI.SetActive(false);
            progressBarAll.SetActive(false);
            progress = 0f;
            progressBar.fillAmount = 0f;
        }
    }
}
