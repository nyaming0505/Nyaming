using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;
    [Header("UI Components")]
    public GameObject gameOverPanel;
    public Text yesText;
    public Text noText;

    private bool isYesSelected = true;

    // 
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Time.timeScale = 1f; 
        gameOverPanel.SetActive(false); 
        isYesSelected = true; 
    }

    void Update()
    {
        if (gameOverPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                isYesSelected = !isYesSelected;
                UpdateCursor();
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                SelectOption();
            }
        }
    }

    // 외부에서 게임오버 시킬 때 이 함수만 부르면 됨
    public void TriggerGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        isYesSelected = true;
        UpdateCursor();
    }

    void UpdateCursor()
    {
        if (isYesSelected)
        {
            yesText.text = "> YES";
            noText.text = "NO";
            yesText.color = Color.yellow;
            noText.color = Color.white;
        }
        else
        {
            yesText.text = "YES";
            noText.text = "> NO";
            yesText.color = Color.white;
            noText.color = Color.yellow;
        }
    }

    void SelectOption()
    {
        Time.timeScale = 1f;

        if (isYesSelected)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
        }
        else
        {
            if (gameOverPanel != null) gameOverPanel.SetActive(false);

            if (TitleManager.Instance != null)
            {
                TitleManager.Instance.ShowTitleScreen();
            }
            else
            {
                Debug.LogError("TitleManager 인스턴스를 찾을 수 없습니다.");
            }
        }
    }
}