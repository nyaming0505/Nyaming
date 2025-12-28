using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameResultPanel : MonoBehaviour
{
    [Header("UI")]
    public Image resultImage;
    public Text resultText;

    [Header("Sprites")]
    public Sprite milkSprite;
    public Sprite waterSprite;
    public Sprite chocoSprite;

    IngredientType currentResult;
    bool canClose = false;
    void OnEnable()
    {
        canClose = false;
        Invoke(nameof(EnableClose), 0.2f); // ³Ê¹« ºü¸¥ ÀÔ·Â ¹æÁö
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!canClose)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Close();
        }
    }

    void EnableClose()
    {
        canClose = true;
    }

    public void ShowResult(IngredientType result)
    {
        currentResult = result;

        switch (result)
        {
            case IngredientType.Milk:
                resultImage.sprite = milkSprite;
                resultText.text = "¿ìÀ¯ È¹µæ¡Ú";
                break;

            case IngredientType.Water:
                resultImage.sprite = waterSprite;
                resultText.text = "¹° È¹µæ¡Ú";
                break;

            case IngredientType.Choco:
                resultImage.sprite = chocoSprite;
                resultText.text = "ÃÊÄÚ È¹µæ¡Ú";
                break;
        }

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);

        MiniGameState.isBusy = false;
        MiniGameState.waitForKeyRelease = true;
    }
}
