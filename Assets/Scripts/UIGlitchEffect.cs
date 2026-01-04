using UnityEngine;
using UnityEngine.UI;

public class UIGlitchEffect : MonoBehaviour
{
    [Header("Current Status")]
    public int glitchLevel = 0;

    [Header("Intensity Settings (강도 조절)")]
    public float level1Shake = 3.0f;  // 1단계: 살짝 떨림
    public float level2Shake = 7.0f;  // 2단계: 꽤 고장남

    public float level1Alpha = 0.01f;  // 투명도 노이즈 (약함)
    public float level2Alpha = 0.02f;  // 투명도 노이즈 (강함)

    [Header("Animation Settings")]
    public float shakeInterval = 0.1f;
    private float shakeTimer = 0f;

    // 내부 변수
    private float currentShake = 0f;
    private float currentAlphaNoise = 0f;

    private Transform targetTransform;
    private Vector3 originalLocalPos;
    private CanvasGroup canvasGroup;
    private bool isInitialized = false;

    private Vector3 shakeOffset;
    private float alphaNoiseValue;

    void Awake()
    {
        targetTransform = transform;
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        if (!isInitialized && targetTransform != null)
        {
            originalLocalPos = targetTransform.localPosition;
            isInitialized = true;
        }
        shakeTimer = 0f;
    }

    void OnDisable()
    {
        if (isInitialized && targetTransform != null)
        {
            targetTransform.localPosition = originalLocalPos;
        }
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
    }

    void Update()
    {
        if (glitchLevel <= 0) return;

        shakeTimer += Time.unscaledDeltaTime;

        if (shakeTimer >= shakeInterval)
        {
            shakeTimer = 0f;

            // 위치 랜덤
            float x = Random.Range(-1f, 1f) * currentShake;
            float y = Random.Range(-1f, 1f) * currentShake;
            shakeOffset = new Vector3(x, y, 0);

            // 투명도 랜덤
            alphaNoiseValue = Random.Range(0f, currentAlphaNoise);
        }
    }

    void LateUpdate()
    {
        if (!isInitialized || targetTransform == null) return;

        if (glitchLevel <= 0)
        {
            targetTransform.localPosition = originalLocalPos;
            if (canvasGroup != null) canvasGroup.alpha = 1f;
            return;
        }

        targetTransform.localPosition = originalLocalPos + shakeOffset;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f - alphaNoiseValue;
        }
    }

    public void SetGlitchLevel(int level)
    {
        glitchLevel = level;

        switch (level)
        {
            case 0:
                currentShake = 0f;
                currentAlphaNoise = 0f;
                // 0레벨이 되면 즉시 복구
                if (isInitialized) targetTransform.localPosition = originalLocalPos;
                if (canvasGroup != null) canvasGroup.alpha = 1f;
                break;

            case 1:
                currentShake = level1Shake;
                currentAlphaNoise = level1Alpha;
                break;

            default:
                currentShake = level2Shake;
                currentAlphaNoise = level2Alpha;
                break;
        }

        Debug.Log($"글리치 효과 설정됨: Level {level}, 강도 {currentShake}");
    }
}