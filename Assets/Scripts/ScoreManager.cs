using UnityEngine;
using UnityEngine.UI; // 레거시 텍스트 및 UI 기능 필수

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI Component")]
    public Text scoreText;

    public RectTransform scoreRect;
    private int currentScore = 0;
    public int scoreMultiplier = 1;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        currentScore = 0;
        scoreMultiplier = 1;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int score)
    {
        currentScore += (score * scoreMultiplier);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString("N0");
        }

        // ★ 추가: 글자가 바뀐 즉시 레이아웃을 다시 계산해서 배경을 늘려줍니다.
        if (scoreRect != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(scoreRect);
        }
    }

    public void ActivateDoubleScoreBug()
    {
        scoreMultiplier = 2;
        Debug.Log("버그 발동! 점수 2배 적용됨");
    }

    public void ResetScore()
    {
        currentScore = 0;
        scoreMultiplier = 1;
        UpdateUI();
        Debug.Log("점수 리셋 완료! 현재 점수: " + currentScore * scoreMultiplier);
    }
}