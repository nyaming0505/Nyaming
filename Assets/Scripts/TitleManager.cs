using System.Collections;
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

    public UIGlitchEffect titleGlitchEffect;

    // static 변수로 게임 상태 유지
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
        if (isGamePlaying)
        {
            if (gameTitleObject != null) gameTitleObject.SetActive(false);
            StartCoroutine(LoadGameSceneAdditive());
        }
        else
        { 
            ShowTitleScreen();
        }
    }

    void Update()
    {
        if (isGamePlaying) return;

        if (inputCooldown > 0)
        {
            inputCooldown -= Time.deltaTime;
            return;
        }

        if (pressText != null)
        {
            Color color = pressText.color;
            color.a = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
            pressText.color = color;
        }

        if (Input.anyKeyDown && !isStarting)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        isStarting = true;
        isGamePlaying = true;

        if (EndingManager.Instance != null)
        {
            EndingManager.Instance.ResetBugs();
        }

        if (HeartManager.instance != null)
        {
            HeartManager.instance.ResetHearts();
        }   
        
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }

        pressText.color = Color.white;

        if (gameTitleObject != null) gameTitleObject.SetActive(false);

        
        StartCoroutine(LoadGameSceneAdditive());
    }

    IEnumerator LoadGameSceneAdditive()
    {
        if (SceneManager.GetSceneByName(gameSceneName).isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(gameSceneName);
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);
    
        while (!op.isDone)
        {
            yield return null;
        }

        Time.timeScale = 1f;
    }

    public void ShowTitleScreen()
    {
        isGamePlaying = false;
        isStarting = false;

        if (gameTitleObject != null)
        {
            gameTitleObject.SetActive(true);
        }

        if (titleGlitchEffect != null && EndingManager.Instance != null)
        {
            titleGlitchEffect.SetGlitchLevel(EndingManager.Instance.bugCount);
            if (EndingManager.Instance.bugCount >= 2)
            {
                pressText.color = Color.red;
            }
            Debug.Log($"타이틀 복귀! 버그 개수: {EndingManager.Instance.bugCount}");
        }

        inputCooldown = 0.5f;
    }
}