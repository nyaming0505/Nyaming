using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI를 다루기 위해 필수!

public class HeartManager : MonoBehaviour
{
    public static HeartManager instance;

    [Header("설정")]
    public int maxHearts = 5;
    public int currentHearts;

    [Header("UI 연결")]
    public Image[] heartImages;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        ResetHearts();
    }

    public void OnOrderFailed()
    {
        if (currentHearts > 0)
        {
            currentHearts--;
            UpdateHeartUI();

            if (currentHearts <= 0)
            {
                GameOver();
            }
        }
    }

    void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHearts)
            {
                heartImages[i].sprite = fullHeartSprite;
            }
            else
            {
                heartImages[i].sprite = emptyHeartSprite;
            }
        }
    }

    void GameOver()
    {
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.TriggerGameOver();
        }
    }

    public void ResetHearts()
    {
        currentHearts = maxHearts;
        UpdateHeartUI();
    }
}