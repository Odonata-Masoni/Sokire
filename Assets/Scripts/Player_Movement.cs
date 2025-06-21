using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    // Các biến di chuyển
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    [SerializeField] private float jumpHeight = 4f; // Chiều cao nhảy
    [SerializeField] private float jumpTimeToApex = 0.35f; // Thời gian đạt đỉnh nhảy
    [SerializeField] private LayerMask groundLayer; // Layer của mặt đất
    [SerializeField] private float groundCheckDistance = 0.1f; // Khoảng cách kiểm tra mặt đất

    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D capsulecollider2D;
    private Vector2 moveInput;
    private float facingDirection = 1f;
    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isRunning = false;
    private float jumpVelocity; // Vận tốc nhảy
    private float gravity; // Trọng lực thủ công


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsulecollider2D = GetComponent<CapsuleCollider2D>();

        // Tính toán vận tốc nhảy và trọng lực
        gravity = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
        jumpVelocity = Mathf.Abs(gravity) * jumpTimeToApex;

        // Đảm bảo Rigidbody2D không sử dụng trọng lực của Unity
        rb.gravityScale = 0f;
    }

    void FixedUpdate()
    {
        // Kiểm tra trạng thái mặt đất
        CheckGround();

        // Xử lý vận tốc
        float moveX = moveInput.x;
        float moveY = rb.velocity.y;

        // Áp dụng tốc độ dựa trên trạng thái chạy
        float currentSpeed = isRunning && Mathf.Abs(moveX) > 0.1f ? runSpeed : moveSpeed;

        // Áp dụng trọng lực thủ công nếu không trên mặt đất
        if (!isGrounded)
        {
            moveY += gravity * Time.fixedDeltaTime;
        }

        // Giới hạn vận tốc rơi
        moveY = Mathf.Max(moveY,-20f);

        // Đặt vận tốc
        rb.velocity = new Vector2(moveX * currentSpeed, moveY);
        // Kiểm tra đạt đỉnh nhảy để chuyển sang trạng thái rơi
        if (rb.velocity.y > 0 && isJumping)
        {
            isFalling = false; // Đang nhảy lên
        }
        else if (rb.velocity.y <= 0 && !isGrounded)
        {
            isJumping = false; // Không còn nhảy
            isFalling = true;  // Đang rơi
        }

        // Đặt lại vận tốc Y khi chạm đất
        if (isGrounded && rb.velocity.y < 0)
        {
            moveY = 0;
            isJumping = false;
        }

        // Cập nhật lật sprite và animation
        Flip(moveX);
        UpdateAnimation(moveX);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        moveInput = moveInput.normalized;
    }
    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.performed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
            isJumping = true;
            isGrounded = false;
        }
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            capsulecollider2D.bounds.center,
            capsulecollider2D.bounds.size,
            0f,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );
        isGrounded = hit.collider != null;
    }

    private void Flip(float moveX)
    {
        if (moveX > 0.1f && facingDirection < 0)
        {
            facingDirection = 1f;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveX < -0.1f && facingDirection > 0)
        {
            facingDirection = -1f;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void UpdateAnimation(float moveX)
    {
        animator.SetBool(AnimationStrings.isMoving, Mathf.Abs(moveX) > 0.1f);
        animator.SetBool(AnimationStrings.isJumping, isJumping);
        animator.SetBool(AnimationStrings.isGrounded, isGrounded);
        animator.SetBool(AnimationStrings.isFalling, isFalling);
        animator.SetBool(AnimationStrings.isRunning, isRunning && Mathf.Abs(moveX) > 0.1f); // Chỉ chạy khi di chuyển và nhấn Shift
    }


}