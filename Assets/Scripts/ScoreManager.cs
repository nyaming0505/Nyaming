using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public static int sessionHighScore = 0;

    [Header("UI Component")]
    public Text scoreText;

    public RectTransform scoreRect;
    public int currentScore = 0;
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

        if (scoreRect != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(scoreRect);
        }
    }

    public void ActivateDoubleScoreBug()
    {
        scoreMultiplier = 2;
    }

    public void ResetScore()
    {
        currentScore = 0;
        scoreMultiplier = 1;
        UpdateUI();
    }
}