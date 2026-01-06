using UnityEngine;

public class CameraGlitchOnly : MonoBehaviour
{
    [Header("Current Status")]
    public int glitchLevel = 0;

    [Header("Shake Intensity (카메라 흔들림 강도)")]
    public float level1Shake = 0.1f;
    public float level2Shake = 0.3f;

    [Header("Animation Settings")]
    public float shakeInterval = 0.05f;
    private float shakeTimer = 0f;

    private Transform mainCamTransform;
    private Vector3 originalCamPos;
    private float currentShakeStrength = 0f;
    private Vector3 shakeOffset;
    private bool isShaking = false;

    void Awake()
    {
        FindMainCamera();
    }

    public void FindMainCamera()
    {
        if (Camera.main != null)
        {
            mainCamTransform = Camera.main.transform;
        }
    }

    void OnDisable()
    {
        RecoverCameraPosition();
    }

    void Update()
    {
        if (glitchLevel <= 0 || mainCamTransform == null) return;

        shakeTimer += Time.unscaledDeltaTime;

        if (shakeTimer >= shakeInterval)
        {
            shakeTimer = 0f;
            float x = Random.Range(-1f, 1f) * currentShakeStrength;
            float y = Random.Range(-1f, 1f) * currentShakeStrength;
            shakeOffset = new Vector3(x, y, 0);
        }
    }

    void LateUpdate()
    {
        if (mainCamTransform == null) return;

        if (glitchLevel > 0)
        {
            mainCamTransform.localPosition = originalCamPos + shakeOffset;
        }
        else if (isShaking)
        {
            RecoverCameraPosition();
        }
    }

    private void RecoverCameraPosition()
    {
        if (mainCamTransform != null && isShaking)
        {
            mainCamTransform.localPosition = originalCamPos;
            isShaking = false;
        }
    }

    public void SetGlitchLevel(int level)
    {
        if (mainCamTransform == null) FindMainCamera();

        if (glitchLevel == 0 && level > 0 && mainCamTransform != null)
        {
            originalCamPos = mainCamTransform.localPosition;
            isShaking = true;
        }

        glitchLevel = level;

        switch (level)
        {
            case 0:
                currentShakeStrength = 0f;
                break;
            case 1:
                currentShakeStrength = level1Shake;
                break;
            default:
                currentShakeStrength = level2Shake;
                break;
        }
    }
}