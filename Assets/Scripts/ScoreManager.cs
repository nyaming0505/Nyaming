using UnityEngine;
using UnityEngine.UI; // 레거시 텍스트 사용 시 필수

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI Component")]
    public Text scoreText;

    private int currentScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentScore = 0;
        UpdateUI();
    }

    public void AddScore(int score)
    {
        currentScore += score;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString("N0");
        }
    }
}