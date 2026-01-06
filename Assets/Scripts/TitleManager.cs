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

    [Header("Camera Effect")]
    public CameraGlitchOnly cameraGlitch;

    public static bool isGamePlaying = false;
    private bool isStarting = false;
    private float inputCooldown = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        FindCameraGlitch();
    }

    void Start()
    {
        StartCoroutine(LoadGameSceneAdditive());

        if (isGamePlaying)
        {
            if (gameTitleObject != null) gameTitleObject.SetActive(false);
        }
        else
        {
            ShowTitleScreen();
        }
    }

    void FindCameraGlitch()
    {
        if (cameraGlitch == null)
            cameraGlitch = FindObjectOfType<CameraGlitchOnly>();

        if (cameraGlitch == null && Camera.main != null)
            cameraGlitch = Camera.main.GetComponent<CameraGlitchOnly>();
    }

    void Update()
    {

        if (!isGamePlaying && pressText != null)
        {
            Color color = pressText.color;
            color.a = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
            pressText.color = color;
        }

        if (isGamePlaying) return;

        if (inputCooldown > 0)
        {
            inputCooldown -= Time.deltaTime;
            return;
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

        if (cameraGlitch != null) cameraGlitch.SetGlitchLevel(0);

        if (EndingManager.Instance != null) EndingManager.Instance.ResetBugs();
        if (HeartManager.instance != null) HeartManager.instance.ResetHearts();
        if (ScoreManager.Instance != null) ScoreManager.Instance.ResetScore();

        if (pressText != null) pressText.color = Color.white;
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

        Scene gameScene = SceneManager.GetSceneByName(gameSceneName);
        if (gameScene.IsValid())
        {
            SceneManager.SetActiveScene(gameScene);
        }

        Time.timeScale = 1f;

        FindCameraGlitch();

        if (!isGamePlaying && EndingManager.Instance != null)
        {
            if (cameraGlitch != null) cameraGlitch.SetGlitchLevel(EndingManager.Instance.bugCount);
        }
    }

    public void BackToTitle()
    {
        ShowTitleScreen();

        StartCoroutine(LoadGameSceneAdditive());
    }

    public void ShowTitleScreen()
    {
        isGamePlaying = false;
        isStarting = false;

        if (gameTitleObject != null) gameTitleObject.SetActive(true);

        if (EndingManager.Instance != null)
        {
            int bugs = EndingManager.Instance.bugCount;
            if (bugs >= 2 && pressText != null) pressText.color = Color.red;
            Debug.Log($"타이틀 복귀! 버그 개수: {bugs}");
        }

        inputCooldown = 0.5f;
    }
}