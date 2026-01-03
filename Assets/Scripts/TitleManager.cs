using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public static TitleManager Instance;

    [Header("Settings")]
    public string gameSceneName = "SampleScene";
    public string uiSceneName = "UIScene";

    [Header("UI Objects")]
    public GameObject gameTitleObject;
    public Text pressText;
    public float blinkSpeed = 2.0f;

    public static bool isGamePlaying = false;
    private bool isStarting = false;

    private float inputCooldown = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 게임 상태에 따라 타이틀 켜기/끄기 결정
        if (isGamePlaying)
        {
            if (gameTitleObject != null) gameTitleObject.SetActive(false);
        }
        else
        {
            ShowTitleScreen();
        }
    }

    void Update()
    {
        if (isGamePlaying) return;

        // 쿨타임 체크
        if (inputCooldown > 0)
        {
            inputCooldown -= Time.deltaTime;
            return;
        }

        // 깜빡임 효과
        if (pressText != null)
        {
            Color color = pressText.color;
            color.a = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
            pressText.color = color;
        }

        // 시작 키 입력
        if (Input.anyKeyDown && !isStarting)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        isStarting = true;
        isGamePlaying = true;

        // 게임 씬 로드 + UI 씬 얹기
        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(uiSceneName, LoadSceneMode.Additive);

        Time.timeScale = 1f;
    }

    // 타이틀 스크린 보여주기
    public void ShowTitleScreen()
    {
        isGamePlaying = false;
        isStarting = false;

        if (gameTitleObject != null)
        {
            gameTitleObject.SetActive(true);
        }

        // 입력 방지 쿨타임
        inputCooldown = 0.5f;
    }
}