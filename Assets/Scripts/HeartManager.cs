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

    void Update()
    {
        // 🧪 [테스트용] 스페이스바를 누르면 손님 주문 실패 상황을 가정함
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("테스트: 주문 실패! 하트 감소!");
            OnOrderFailed();
        }
    }


    // 손님 주문 실패 시 호출될 함수
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

    // UI 이미지를 현재 하트 개수에 맞춰 바꿔주는 함수
    void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHearts)
            {
                // 현재 하트 개수보다 인덱스가 작으면 꽉 찬 하트
                heartImages[i].sprite = fullHeartSprite;
            }
            else
            {
                // 그 외에는 빈 하트
                heartImages[i].sprite = emptyHeartSprite;
            }
        }
    }

    // 게임오버 처리
    void GameOver()
    {
        Debug.Log("💀 하트 0개! 게임 오버!");

        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.TriggerGameOver();
        }
        else
        {
            Debug.LogError("GameOverManager 인스턴스를 찾을 수 없습니다!");
        }
    }

    public void ResetHearts()
    {
        currentHearts = maxHearts;
        UpdateHeartUI();
        Debug.Log("하트 리셋 완료! 현재 하트: " + currentHearts);
    }
}