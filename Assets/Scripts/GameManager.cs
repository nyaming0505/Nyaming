using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text scoreText;
    public GameObject gameOverPanel;

    private bool isGameActive = true;
    private int currentScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        isGameActive = true;
        currentScore = 0;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        UpdateUI();
    }

    public void AddScore(int score)
    {
        if (!isGameActive) return;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddScore(1000);
        }
    }

    public void EndGame()
    {
        isGameActive = false;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }
}