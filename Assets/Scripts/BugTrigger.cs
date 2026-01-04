using UnityEngine;
using UnityEngine.UI; 
using System.Collections; 

public class BugTrigger : MonoBehaviour
{
    public enum BugType
    {
        ScoreDouble, // 계산대 (점수 2배)
        SpeedDouble, // 쓰레기통 (속도 2배)
        TimeDouble   // 음료 제조대 ( 이동속도 2배)
    }

    [Header("버그 설정")]
    public BugType bugType;
    public int requiredClicks = 10;

    [Header("시간 제한 설정 (쓰레기통용)")]
    public bool useTimeLimit = false;
    public float timeLimit = 5.0f;

    [Header("UI 알림 설정")]
    public Text noticeText;
    public float displayDuration = 2.0f;

    private int currentClicks = 0;
    private float firstClickTime = 0f;
    private bool bugActivated = false;

    void OnEnable()
    {
        ResetBug();
        if (noticeText != null) noticeText.gameObject.SetActive(false);
    }

    public void ResetBug()
    {
        currentClicks = 0;
        firstClickTime = 0f;
        bugActivated = false;
        Debug.Log("버그 전부 초기화됨");
    }

    private void OnMouseDown()
    {
        if (bugActivated) return;

        if (useTimeLimit)
        {
            if (currentClicks == 0)
            {
                firstClickTime = Time.time;
            }
            else if (Time.time - firstClickTime > timeLimit)
            {
                currentClicks = 0;
                firstClickTime = Time.time;
                Debug.Log("시간 초과! 클릭 횟수 초기화");
            }
        }

        currentClicks++;
        Debug.Log($"{gameObject.name} 클릭됨: {currentClicks}/{requiredClicks}");

        if (currentClicks >= requiredClicks)
        {
            ActivateBug();
        }
    }

    void ActivateBug()
    {
        bugActivated = true;

        if (EndingManager.Instance != null)
        {
            EndingManager.Instance.AddBugCount();
        }

        if (noticeText != null)
        {
            StartCoroutine(ShowNoticeRoutine());
        }
        else
        {
            Debug.LogWarning("Notice Text가 연결되지 않았습니다!");
        }

        switch (bugType)
        {
            case BugType.ScoreDouble:
                noticeText.text = "SYSTEM ERROR : 점수 2배";
                noticeText.color = Color.yellow;
                if (ScoreManager.Instance != null)
                    ScoreManager.Instance.ActivateDoubleScoreBug();
                break;

            case BugType.SpeedDouble:
                noticeText.text = "SYSTEM ERROR : 속도 2배";
                noticeText.color = Color.red;
                if (PlayerMovement.Instance != null)
                    PlayerMovement.Instance.ActivateSpeedBug();
                break;

            case BugType.TimeDouble:
                noticeText.text = "SYSTEM ERROR : 시간 2배";
                noticeText.color = new Color(0.7f, 0.2f, 1.0f);
                if (OrderManager.Instance != null)
                    OrderManager.Instance.ActivateDoubleTimeBug();
                break;
        }
    }

    IEnumerator ShowNoticeRoutine()
    {
        noticeText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        noticeText.gameObject.SetActive(false);
    }
}