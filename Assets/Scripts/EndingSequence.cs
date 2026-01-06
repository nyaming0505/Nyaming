using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class EndingSequence : MonoBehaviour
{
    [Header("UI 연결")]
    public CanvasGroup fadePanel;
    public Text codeText;
    public Text finalText;
    public VideoPlayer videoPlayer;
    public GameObject endingUIRoot;

    [Header("설정")]
    public float fadeDuration = 2.0f;

    private string[] systemLogs = new string[]
    {
        "Initialize System...",
        "Loading Resources...",
        "Memory Segment: 0x4E5941...",   // NYA
        "Memory Segment: 0x4D494E...",   // MIN
        "Memory Segment: 0x470505...",   // G + 0505
        "> Memory Access: SUCCESS",
        "User Permission: Granted...",
        "Connection Established.",
        "Upload Complete."
    };

    void Start()
    {
        if (fadePanel != null) fadePanel.alpha = 0f;
        if (codeText != null) codeText.text = "";
        if (finalText != null)
        {
            finalText.text = "";
            finalText.gameObject.SetActive(false);
        }
    }

    public void PlayEnding()
    {
        Debug.Log("🎬 [EndingSequence] 진엔딩 시퀀스 시작!");
        if (endingUIRoot != null)
        {
            endingUIRoot.SetActive(true);

            Canvas canvas = endingUIRoot.GetComponent<Canvas>();
            if (canvas == null) canvas = endingUIRoot.GetComponentInParent<Canvas>();
            if (canvas != null) canvas.sortingOrder = 999;
        }

        StartCoroutine(ProcessSequence());
    }

    IEnumerator ProcessSequence()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            if (fadePanel != null) fadePanel.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        if (fadePanel != null) fadePanel.alpha = 1f;

        yield return new WaitForSecondsRealtime(0.5f);

        if (codeText != null)
        {
            codeText.gameObject.SetActive(true);
            foreach (string line in systemLogs)
            {
                codeText.text += "> ";
                foreach (char letter in line.ToCharArray())
                {
                    codeText.text += letter;
                    yield return new WaitForSecondsRealtime(0.03f);
                }
                codeText.text += "\n";
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        yield return new WaitForSecondsRealtime(1.0f);

        if (codeText != null) codeText.gameObject.SetActive(false);

        if (finalText != null)
        {
            finalText.gameObject.SetActive(true);
            string lastMsg = "System.Nyaming = online";

            foreach (char letter in lastMsg.ToCharArray())
            {
                finalText.text += letter;
                yield return new WaitForSecondsRealtime(0.05f);
            }
            Debug.Log("3️⃣ 로그 출력 완료");
        }

        yield return new WaitForSecondsRealtime(3.0f);

        Debug.Log("4️⃣ 동영상 재생 시도");
        if (finalText != null) finalText.gameObject.SetActive(false);

        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(true);
            videoPlayer.Play();
        }
    }
}