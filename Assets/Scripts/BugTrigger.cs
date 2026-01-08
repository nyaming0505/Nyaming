using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static SoundManager;

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
    public GameObject bugUI;
    public Text noticeText;
    public float displayDuration = 2.0f;

    [Header("커서 설정")]
    public Texture2D hoverCursor;
    public Vector2 hotSpot = new Vector2(12, 12);

    private int currentClicks = 0;
    private float firstClickTime = 0f;
    private bool bugActivated = false;

    private bool isHovering = false;

    void OnEnable()
    {
        ResetBug();
        if (noticeText != null)
        {
            if (bugUI) bugUI.SetActive(false);
            if (noticeText) noticeText.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void ResetBug()
    {
        currentClicks = 0;
        firstClickTime = 0f;
        bugActivated = false;
        isHovering = false;
    }

    void Update()
    {
        if (bugActivated || Time.timeScale == 0f) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D myCollider = GetComponent<Collider2D>();
        bool hit = myCollider.OverlapPoint(mousePos);

        if (hit && !isHovering)
        {
            isHovering = true;
            Cursor.SetCursor(hoverCursor, hotSpot, CursorMode.ForceSoftware);
        }
        else if (!hit && isHovering)
        {
            isHovering = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
        }

        if (isHovering && Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    void HandleClick()
    {
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
            }
        }

        currentClicks++;

        if (currentClicks >= requiredClicks)
        {
            ActivateBug();
            Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    void ActivateBug()
    {
        bugActivated = true;

        if (SoundManager.Instance != null) SoundManager.Instance.PlaySFX(SFXType.Error);

        if (EndingManager.Instance != null)
        {
            EndingManager.Instance.AddBugCount();
        }

        if (noticeText != null)
        {
            StartCoroutine(ShowNoticeRoutine());
        }

        switch (bugType)
        {
            case BugType.ScoreDouble:
                noticeText.text = "SYSTEM ERROR : 점수 2배";
                noticeText.color = Color.red;
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
                noticeText.color = Color.red;
                if (OrderManager.Instance != null)
                    OrderManager.Instance.ActivateDoubleTimeBug();
                break;
        }
    }

    IEnumerator ShowNoticeRoutine()
    {
        bugUI.SetActive(true);
        noticeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        bugUI.SetActive(false);
        noticeText.gameObject.SetActive(false);
    }
}