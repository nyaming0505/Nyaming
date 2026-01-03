using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject gameOverPanel;
    public GameObject gameTitleObject;
    public TitleManager titleManager;
    public Text yesText;
    public Text noText;

    private bool isYesSelected = true;

    // 
    void Awake()
    {
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
            gameOverPanel.SetActive(false);

            if (gameTitleObject != null)
            {
                titleManager.ShowTitleScreen();
            }
        }
    }
}