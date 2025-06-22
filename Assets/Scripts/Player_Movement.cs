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

    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider2D;
    private Vector2 moveInput;
    private float facingDirection = 1f;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isRunning = false;
    private float jumpVelocity; // Vận tốc nhảy
    private float gravity; // Trọng lực thủ công

    private CollisionChecker collisionChecker; // Tham chiếu đến CollisionChecker

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        collisionChecker = GetComponent<CollisionChecker>(); // Lấy component CollisionChecker

        // Tính toán vận tốc nhảy và trọng lực
        gravity = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
        jumpVelocity = Mathf.Abs(gravity) * jumpTimeToApex;

        // Đảm bảo Rigidbody2D không sử dụng trọng lực của Unity
        rb.gravityScale = 0f;

        // Gán giá trị cho CollisionChecker
        collisionChecker.collisionLayer = groundLayer;
    }

    void FixedUpdate()
    {
        // Kiểm tra tất cả va chạm
        collisionChecker.CheckCollisions();

        // Xử lý vận tốc
        float moveX = moveInput.x;
        float moveY = rb.velocity.y;

        // Áp dụng tốc độ dựa trên trạng thái chạy
        float currentSpeed = isRunning && Mathf.Abs(moveX) > 0.1f ? runSpeed : moveSpeed;

        // Ngăn di chuyển qua tường
        if (collisionChecker.IsTouchingWall && Mathf.Abs(moveX) > 0.1f)
        {
            moveX = 0; // Dừng di chuyển khi chạm tường
        }

        // Áp dụng trọng lực thủ công nếu không trên mặt đất và không chạm trần
        if (!collisionChecker.IsGrounded && !collisionChecker.IsTouchingCeiling)
        {
            moveY += gravity * Time.fixedDeltaTime;
        }

        // Giới hạn vận tốc rơi
        moveY = Mathf.Max(moveY, -20f);

        // Điều chỉnh di chuyển khi gần mép vực
        if (collisionChecker.IsNearCliff && Mathf.Abs(moveX) > 0.1f)
        {
            currentSpeed *= 0.5f; // Giảm tốc độ khi gần mép vực
        }

        // Đặt vận tốc
        rb.velocity = new Vector2(moveX * currentSpeed, moveY);

        // Kiểm tra trạng thái
        if (collisionChecker.IsGrounded)
        {
            isFalling = false; // Không rơi khi chạm đất
        }
        else if (rb.velocity.y > 0 && isJumping)
        {
            isFalling = false; // Đang nhảy lên
        }
        else if (rb.velocity.y < 0 && !collisionChecker.IsGrounded)
        {
            isJumping = false; // Không còn nhảy
            isFalling = true;  // Đang rơi
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
        if (context.performed && collisionChecker.IsGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
            isJumping = true;
        }
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
        animator.SetBool(AnimationStrings.isFalling, isFalling);
        animator.SetBool(AnimationStrings.isRunning, isRunning && Mathf.Abs(moveX) > 0.1f);
    }
}