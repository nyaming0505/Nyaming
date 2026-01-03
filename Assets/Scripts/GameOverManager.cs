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

    [Header("팁 시스템 설정")]
    public Text tipTitle;
    public Text tipText;

    // 0.0 ~ 1.0 사이 값 조절 (0.5 = 50% 확률로 팁 등장, 나머지는 안 나옴)
    [Range(0f, 1f)] public float tipChance = 0.7f;

    public string[] gameTips;

    private bool isYesSelected = true;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Time.timeScale = 1f;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        isYesSelected = true;

        // 팁 내용 자동 설정 (비어있으면 채워넣음)
        if (gameTips == null || gameTips.Length == 0)
        {
            // 팁 관련 배열 (수정하면됨!)
            gameTips = new string[]
            {
                "시간이 너무 부족한가요? 전자렌지를 연타하면...",
                "점수가 너무 부족한가요? 계산대를 연타하면...",
                "이 게임은 완벽해요! 버그는 절대 존재하지 않아요!",
                "냐밍을 쓰다듬어주면 무언가 일어난다는 소문이...",
                "냐밍은 가끔 당신을 쳐다보고 있어요.",
                "고양이 관련 지식 뭐시기~~",
                "고양이 관련 지식 뭐시기~~뭐시기~~",
                "고양이 관련 지식 뭐시기~~뭐시기~~뭐시기~~",
                "고양이 관련 지식 뭐시기~~뭐시기~~뭐시기~~뭐시기~~",
                "고양이 관련 지식 뭐시기~~뭐시기~~뭐시기~~뭐시기~~"
            };
        }
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
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            gameOverPanel.transform.SetAsLastSibling();
        }

        ShowRandomTip();

        Time.timeScale = 0f;
        isYesSelected = true;
        UpdateCursor();
    }

    // 팁 출력 함수
    void ShowRandomTip()
    {
        if (tipText == null) return;

        bool showTip = (Random.value <= tipChance);

        if (showTip && gameTips.Length > 0)
        {
            int randomIndex = Random.Range(0, gameTips.Length);
            tipTitle.gameObject.SetActive(true);
            tipText.gameObject.SetActive(true);
            tipText.text = gameTips[randomIndex];
        }
        else
        {
            tipTitle.gameObject.SetActive(false);
            tipText.gameObject.SetActive(false);
        }
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
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
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
                Debug.LogWarning("TitleManager가 없어서 UI 씬을 다시 로드합니다.");
                SceneManager.LoadScene("UIScene", LoadSceneMode.Single);
            }
        }
    }
}