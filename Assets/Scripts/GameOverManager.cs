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
    private string defaultTipTitle = "TIP";

    [Header("진엔딩 UI")]
    public GameObject normalGameOverPanel;
    public GameObject trueEndingPanel;

    public UIGlitchEffect glitchEffect;

    [Range(0f, 1f)] public float tipChance = 0.8f;

    private string[] currentTips;

    private bool isYesSelected = true;

    private readonly string[] defaultTips = new string[]
    {
        "미니게임도 빠른데 주문시간도 빠르네... 마우스로...",
        "계산대가 고장났어요.. 마우스로 클릭해서 고쳐주실래요?",
        "쓰레기통을 여러번 클릭하면... ",
        "마우스도 사용할수 있는 부분이 있어요!",
        "이 게임은 완벽해요! 버그는 절대 존재하지 않아요!",
        "냐밍을 쓰다듬어주면 무언가 일어난다는 소문이...",
        "냐밍은 가끔 당신을 쳐다보고 있어요.",
        "냐밍은 고양이를 좋아한다.",
        "고양이는 완벽한 생물이다."
    };

    private readonly string[] bugLevel1Tips = new string[]
    {
        "냐밍이 불안에 떨고있어요...",
        "계산대가 이상한거 같아요...",
        "쓰레기통이 이상한거 같아요...",
        "재료선반이 이상한거 같아요..."
    };

    private readonly string[] bugLevel2Tips = new string[]
    {
        "System.NullReferenceException: 'Nyaming' does not exist.",
        "데이터 손상됨. 데이터 손상됨. 데이터 손상됨.",
        "01001000 01000101 01001100 01010000",
        "하나 남았어요",
        "ERROR MESSAGE : ERROR MESSAGE : ERROR MESSAGE : ERROR MESSAGE"
    };

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Time.timeScale = 1f;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (tipTitle != null) defaultTipTitle = tipTitle.text;

        currentTips = defaultTips;
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
        int bugs = (EndingManager.Instance != null) ? EndingManager.Instance.bugCount : 0;

        Debug.Log($"[GameOver] 버그 개수: {bugs}");

        if (bugs >= 3)
        {
            if (trueEndingPanel != null)
            {
                trueEndingPanel.SetActive(true);
                trueEndingPanel.transform.SetAsLastSibling();

                EndingSequence sequence = trueEndingPanel.GetComponent<EndingSequence>();
                if (sequence != null)
                {
                    sequence.PlayEnding();
                }
            }
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                gameOverPanel.transform.SetAsLastSibling();

                if (glitchEffect == null) glitchEffect = gameOverPanel.GetComponent<UIGlitchEffect>();
            }

            SetBugLevelData(bugs);

            if (glitchEffect != null)
            {
                glitchEffect.SetGlitchLevel(bugs);
            }

            ShowRandomTip();
        }

        Time.timeScale = 0f;
        isYesSelected = true;
        UpdateCursor();
    }

    void SetBugLevelData(int bugs)
    {
        ResetUIColors();
        tipChance = 0.8f;

        switch (bugs)
        {
            case 1:
                currentTips = bugLevel1Tips;
                tipChance = 1f;
                break;

            case 2:
                currentTips = bugLevel2Tips;
                tipChance = 1f;

                if (tipTitle != null)
                {
                    tipTitle.text = "FATAL_ERROR : 0x0505";
                    tipTitle.color = Color.red;
                }
                if (tipText != null) tipText.color = Color.red;
                break;

            default:
                currentTips = defaultTips;
                break;
        }
    }

    void ShowRandomTip()
    {
        if (tipText == null || tipTitle == null) return;

        bool showTip = (Random.value <= tipChance);

        if (showTip && currentTips != null && currentTips.Length > 0)
        {
            int randomIndex = Random.Range(0, currentTips.Length);

            tipTitle.gameObject.SetActive(true);
            tipText.gameObject.SetActive(true);
            tipText.text = currentTips[randomIndex];
        }
        else
        {
            tipTitle.gameObject.SetActive(false);
            tipText.gameObject.SetActive(false);
        }
    }

    void UpdateCursor()
    {
        yesText.text = isYesSelected ? "> YES" : "YES";
        noText.text = isYesSelected ? "NO" : "> NO";

        yesText.color = isYesSelected ? Color.yellow : Color.white;
        noText.color = isYesSelected ? Color.white : Color.yellow;
    }

    void SelectOption()
    {
        Time.timeScale = 1f;

        ResetUIColors();

        if (isYesSelected)
        {
            if (EndingManager.Instance != null)
            {
                EndingManager.Instance.ResetBugs();
            }
            SceneManager.LoadScene("UIScene", LoadSceneMode.Single);
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

    void ResetUIColors()
    {
        if (tipTitle != null)
        {
            tipTitle.text = defaultTipTitle;
            tipTitle.color = Color.white;
        }
        if (tipText != null) tipText.color = Color.white;
    }
}