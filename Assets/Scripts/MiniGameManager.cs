using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MiniGameIngredient
{
    Water,
    Milk,
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
    public float rotateSpeed;
    public float limitAngle = 60f;

    float currentAngle = 0f;
    int direction = 1;
    bool isPlaying = false;

    public MiniGameResultPanel resultPanel;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        isPlaying = true;

        rotateSpeed = LevelManager.Instance.GetMiniGameRotateSpeed();

        currentAngle = Random.Range(-limitAngle, limitAngle);
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
            MiniGameIngredient result = GetIngredientByAngle();
            Debug.Log("È¹µæ Àç·á: " + result);

            CupIngredient cupIngredient = ConvertToCupIngredient(result);
            CupManager.Instance.AddIngredient(cupIngredient);

            resultPanel.ShowResult(result);

            gameObject.SetActive(false);
        }
    }

    void RotateArrow()
    {
        currentAngle += rotateSpeed * direction * Time.deltaTime;

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

    MiniGameIngredient GetIngredientByAngle()
    {
        if (currentAngle <= 60f && currentAngle > 20f)
            return MiniGameIngredient.Milk;
        else if (currentAngle <= 20f && currentAngle > -20f)
            return MiniGameIngredient.Water;
        else
            return MiniGameIngredient.Choco;
    }

    CupIngredient ConvertToCupIngredient(MiniGameIngredient mini)
    {
        switch (mini)
        {
            case MiniGameIngredient.Milk:
                return CupIngredient.Milk;
            case MiniGameIngredient.Water:
                return CupIngredient.Water;
            case MiniGameIngredient.Choco:
                return CupIngredient.Choco;
            default:
                return CupIngredient.Water;
        }
    }
}
