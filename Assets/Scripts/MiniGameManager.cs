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
    [Header("Arrow")]
    public Transform arrow;
    public float rotateSpeed = 120f;

    float currentAngle = 0f;
    int direction = 1; // 1 = ½Ã°è, -1 = ¹Ý½Ã°è
    bool isPlaying = false;

    public MiniGameResultPanel resultPanel;


    private void Start()
    {
        gameObject.SetActive(false); ;
    }

    void OnEnable()
    {
        isPlaying = true;
        currentAngle = 0f;
        direction = 1;
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
            Debug.Log("È¹µæ Àç·á: " + result);

            resultPanel.ShowResult(result);
            // CupManager.Instance.AddIngredient(result);

            gameObject.SetActive(false);
        }
    }

    void RotateArrow()
    {
        currentAngle += rotateSpeed * direction * Time.deltaTime;

        if (currentAngle >= 60f)
        {
            currentAngle = 60f;
            direction = -1;
        }
        else if (currentAngle <= -60f)
        {
            currentAngle = -60f;
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
        if (currentAngle <= 60f && currentAngle > 20f)
            return IngredientType.Milk;
        else if (currentAngle <= 20f && currentAngle > -20f)
            return IngredientType.Water;
        else
            return IngredientType.Choco;
    }

}
