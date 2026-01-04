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

    void Awake()
    {
        emotionIcon.sprite = silentSprite;
        orderIcon.gameObject.SetActive(false) ;
        emotionIcon.gameObject.SetActive(true);
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
