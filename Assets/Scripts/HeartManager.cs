using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI를 다루기 위해 필수!

public class HeartManager : MonoBehaviour
{
    // 어디서든 이 매니저를 부를 수 있게 싱글톤 처리 (나중에 손님 스크립트에서 쓰기 편함)
    public static HeartManager instance;

    [Header("설정")]
    public int maxHearts = 5;       // 최대 하트 개수
    public int currentHearts;       // 현재 하트 개수

    [Header("UI 연결")]
    public Image[] heartImages;     // 하트 이미지 5개가 들어갈 배열
    public Sprite fullHeartSprite;  // 꽉 찬 하트 그림
    public Sprite emptyHeartSprite; // 빈 하트 그림

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        // 게임 시작 시 하트 꽉 채우기
        currentHearts = maxHearts;
        UpdateHeartUI();
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
            currentHearts--; // 하트 1개 감소
            UpdateHeartUI(); // UI 갱신

            // 하트가 0이 되었는지 확인
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

        // 씬에 있는 GameOverManager를 찾아서 실행함
        GameOverManager gm = FindObjectOfType<GameOverManager>();

        if (gm != null)
        {
            gm.TriggerGameOver(); // 게임오버 패널 띄우기
        }
        else
        {
            Debug.LogError("씬에 GameOverManager가 없습니다!");
        }
    }
}