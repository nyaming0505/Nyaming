using UnityEngine;
using UnityEngine.UI;

public class CustomerBubbleUI : MonoBehaviour
{
    [Header("Images")]
    public Image orderIcon;     // 주문 음료
    public Image emotionIcon;   // 성공/실패 이모션

    [Header("Emotion Sprites")]
    public Sprite silentSprite;
    public Sprite EmptySprite;
    public Sprite successSprite;
    public Sprite failSprite;
    public Sprite angrySprite;

    [Header("Queue Gauge")]
    public GameObject waitGaugeRoot;   // 전체 게이지 오브젝트
    public Image waitGaugeFill;        // Fill 이미지
    void Awake()
    {
        emotionIcon.sprite = silentSprite;
        orderIcon.gameObject.SetActive(false) ;
        emotionIcon.gameObject.SetActive(true);
        waitGaugeRoot.SetActive(false);
    }
    public void ShowAngry()
    {
        orderIcon.gameObject.SetActive(false);
        emotionIcon.sprite = angrySprite;
        emotionIcon.gameObject.SetActive(true);
    }

    // 대기 시작
    public void ShowWaitGauge()
    {
        waitGaugeRoot.SetActive(true);
        SetWaitGauge(1f);
    }

    // 값 갱신 (0~1)
    public void SetWaitGauge(float ratio)
    {
        waitGaugeFill.fillAmount = Mathf.Clamp01(ratio);
    }

    // 대기 종료
    public void HideWaitGauge()
    {
        waitGaugeRoot.SetActive(false);
    }

    // =========================
    // 주문 받았을 때
    // =========================
    public void ShowOrder(Sprite recipeSprite)
    {
        orderIcon.sprite = recipeSprite;
        emotionIcon.sprite = EmptySprite;
        orderIcon.gameObject.SetActive(true);
        emotionIcon.gameObject.SetActive(true);
    }

    // =========================
    // 성공 / 실패 처리
    // =========================
    public void ShowResult(bool success)
    {
        orderIcon.gameObject.SetActive(false);

        emotionIcon.sprite = success ? successSprite : failSprite;
        emotionIcon.gameObject.SetActive(true);
    }

    public void ClearAll()
    {
        orderIcon.gameObject.SetActive(false);
        emotionIcon.gameObject.SetActive(false);
    }
}
