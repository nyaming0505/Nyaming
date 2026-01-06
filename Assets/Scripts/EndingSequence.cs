using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class EndingSequence : MonoBehaviour
{
    [Header("UI 연결")]
    public CanvasGroup fadePanel;
    public Text codeText;
    public VideoPlayer videoPlayer;
    public GameObject endingUIRoot; // 껐다 켰다 할 부모 오브젝트

    [Header("설정")]
    public float fadeDuration = 2.0f;

    private string[] systemLogs = new string[]
    {
        "Initialize System...",
        "Loading Resources...",
        "Memory Access: 0x84F2A...",
        "User Permission: Granted...",
        "Target: Nayaming...",
        "Upload Complete."
    };

    // 시작할 때 UI 숨기기
    void Start()
    {
        if (endingUIRoot != null) endingUIRoot.SetActive(false);
    }

    // ⭐ EndingManager가 이 함수를 부를 겁니다
    public void PlayEnding()
    {
        if (endingUIRoot != null) endingUIRoot.SetActive(true);
        StartCoroutine(ProcessSequence());
    }

    IEnumerator ProcessSequence()
    {
        // 1. 페이드 아웃 (화면 점점 검게)
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            if (fadePanel != null) fadePanel.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        if (fadePanel != null) fadePanel.alpha = 1f;

        yield return new WaitForSeconds(0.5f);

        // 2. 시스템 로그 출력 (타자기 효과)
        if (codeText != null)
        {
            codeText.text = "";
            foreach (string line in systemLogs)
            {
                codeText.text += "> " + line + "\n";
                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(1.0f);

            // 3. 핵심 메시지
            codeText.text += "\n> System.Nayaming = online";
        }

        // 4. 3초 대기
        yield return new WaitForSeconds(3.0f);

        // 5. 텍스트 끄고 동영상 재생
        if (codeText != null) codeText.gameObject.SetActive(false);

        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(true); // 혹시 꺼져있으면 켜기
            videoPlayer.Play();
        }
    }
}