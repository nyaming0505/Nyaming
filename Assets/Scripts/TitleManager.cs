using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
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

    void Start()
    {
        if (isGamePlaying == true)
        {
            if (gameTitleObject != null) gameTitleObject.SetActive(false);
        }
        else
        {
            // 처음 시작할 때 타이틀 켜기
            ShowTitleScreen();
        }
    }

    void Update()
    {
        if (isGamePlaying == true) return;

        // 쿨타임이 남아있다면 시간을 줄이고, 아무것도 하지 않음 (리턴)
        if (inputCooldown > 0)
        {
            inputCooldown -= Time.deltaTime;
            return;
        }

        // 글자 깜빡임
        if (pressText != null)
        {
            Color color = pressText.color;
            color.a = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
            pressText.color = color;
        }

        // 아무 키나 눌러서 시작
        if (Input.anyKeyDown && isStarting == false)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        isStarting = true;
        isGamePlaying = true;
        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(uiSceneName, LoadSceneMode.Additive);
        Time.timeScale = 1f;
    }

    // 호출용 함수
    public void ShowTitleScreen()
    {
        isGamePlaying = false;
        isStarting = false;

        if (gameTitleObject != null)
        {
            gameTitleObject.SetActive(true);
        }

        // 타이틀이 켜질 때 0.5초 동안 입력 금지 설정!
        inputCooldown = 0.5f;
    }
}