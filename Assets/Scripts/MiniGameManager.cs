using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    Milk,
    Water,
    Choco
}

public static class MiniGameState
{
    public static bool isBusy = false;
    public static bool waitForKeyRelease = false;
}

public class MiniGameManager : MonoBehaviour
{
    [Header("Arrow Settings")]
    public Transform arrow;
    public float rotateSpeed = 120f;
    public float limitAngle = 60f; // 각도 제한을 변수로 분리 (수정하기 편하도록)

    float currentAngle = 0f;
    int direction = 1; // 1 = 시계, -1 = 반시계
    bool isPlaying = false;

    public MiniGameResultPanel resultPanel;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        isPlaying = true;

        // [수정됨] 시작 각도를 제한 범위 내에서 랜덤으로 설정 (-60 ~ 60)
        currentAngle = Random.Range(-limitAngle, limitAngle);

        // [수정됨] 시작 방향도 랜덤으로 설정 (50% 확률)
        // Random.value는 0.0 ~ 1.0 사이의 랜덤 float 반환
        direction = (Random.value > 0.5f) ? 1 : -1;

        SetArrowRotation();
    }

    void OnDisable()
    {
        isPlaying = false;
    }

    void Update()
    {
        if (!isPlaying)
            return;

        RotateArrow();

        if (Input.GetKeyDown(KeyCode.E))
        {
            IngredientType result = GetIngredientByAngle();
            Debug.Log("획득 재료: " + result);

            resultPanel.ShowResult(result);
            // CupManager.Instance.AddIngredient(result);

            gameObject.SetActive(false);
        }
    }

    void RotateArrow()
    {
        currentAngle += rotateSpeed * direction * Time.deltaTime;

        // [수정됨] 하드코딩된 60f 대신 변수 limitAngle 사용
        if (currentAngle >= limitAngle)
        {
            currentAngle = limitAngle;
            direction = -1;
        }
        else if (currentAngle <= -limitAngle)
        {
            currentAngle = -limitAngle;
            direction = 1;
        }

        SetArrowRotation();
    }

    void SetArrowRotation()
    {
        arrow.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }

    IngredientType GetIngredientByAngle()
    {
        // [참고] 여기의 범위 조건들도 limitAngle 비율에 맞게 조정할 수도 있지만,
        // 일단 기존 로직(60, 20 기준)을 그대로 두었습니다.

        if (currentAngle <= 60f && currentAngle > 20f)
            return IngredientType.Milk;
        else if (currentAngle <= 20f && currentAngle > -20f)
            return IngredientType.Water;
        else
            return IngredientType.Choco;
    }
}