using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("Movement Settings")]
    public float speed = 5f;              // 이동 속도
    public float inputThreshold = 0.1f;   // 입력 감지 최소값

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private Vector2 input;       // 현재 입력값
    private Vector2 lastMove;    // 마지막으로 이동한 방향 (Idle 용)

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 🔹 1) 기본 Axis 입력 (방향키 / WASD)
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // 🔹 1-1) 한글 자판 ㅈㅁㄴㅇ 대응 (물리 키 기준)
        if (Input.GetKey(KeyCode.A)) x = -1f; // ㅁ
        if (Input.GetKey(KeyCode.D)) x = 1f;  // ㅇ
        if (Input.GetKey(KeyCode.W)) y = 1f;  // ㅈ
        if (Input.GetKey(KeyCode.S)) y = -1f; // ㄴ

        // 🔹 2) 대각선 이동 금지
        if (x != 0f) y = 0f;
        else if (y != 0f) x = 0f;

        input = new Vector2(x, y);

        // 🔹 3) 걷고 있는지 판단
        bool isWalking = input.sqrMagnitude > (inputThreshold * inputThreshold);
        anim.SetBool("isWalking", isWalking);

        // 🔹 4) 이동 방향 전달
        anim.SetFloat("moveX", input.x);
        anim.SetFloat("moveY", input.y);

        // 🔹 5) 마지막 이동 방향 기록
        if (isWalking)
        {
            lastMove = input.normalized;
            anim.SetFloat("lastMoveX", lastMove.x);
            anim.SetFloat("lastMoveY", lastMove.y);
        }

        // 🔹 6) 좌우 반전
        if (input.x != 0)
            sr.flipX = input.x < 0;
        else
            sr.flipX = lastMove.x < 0;
    }


    void FixedUpdate()
    {
        // 🔹 실제 이동 처리
        rb.velocity = input * speed;
    }

    public void ActivateSpeedBug()
    {
        speed *= 2f;
    }
}
