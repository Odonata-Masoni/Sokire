using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [Header("Di chuyển")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float jumpTimeToApex = 0.35f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider2D;
    private CollisionChecker collisionChecker;

    private Vector2 moveInput;
    private float facingDirection = 1f;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isRunning = false;
    private float jumpVelocity;
    private float gravity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        collisionChecker = GetComponent<CollisionChecker>();

        gravity = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
        jumpVelocity = Mathf.Abs(gravity) * jumpTimeToApex;
        rb.gravityScale = 0f;
        collisionChecker.collisionLayer = groundLayer;
    }

    void FixedUpdate()
    {
        collisionChecker.CheckCollisions();

        float moveX = moveInput.x;
        float moveY = rb.velocity.y;

        float currentSpeed = isRunning && Mathf.Abs(moveX) > 0.1f ? runSpeed : moveSpeed;

        // Nếu đang chạm trần và vẫn còn vận tốc nhảy lên
        if (collisionChecker.IsTouchingCeiling && moveY > 0f)
        {
            moveY = -1f;
        }

        // Nếu không chạm đất và không bị cắt nhảy bởi trần → áp dụng trọng lực
        if (!collisionChecker.IsGrounded && !collisionChecker.IsTouchingCeiling)
        {
            moveY += gravity * Time.fixedDeltaTime;
        }

        // ❗ Nếu đang chạm trần + vẫn giữ nút di chuyển → tạm thời tắt moveX để tránh “dính”
        if (collisionChecker.IsTouchingCeiling && Mathf.Abs(moveX) > 0.1f)
        {
            moveX = 0f;
        }

        // ❗ Nếu đang chạm tường khi nhảy lên (không dưới đất) và vẫn giữ nút → cũng tắt moveX
        if (collisionChecker.IsTouchingWall && !collisionChecker.IsGrounded && Mathf.Abs(moveX) > 0.1f)
        {
            moveX = 0f;
        }

        moveY = Mathf.Max(moveY, -20f);
        rb.velocity = new Vector2(moveX * currentSpeed, moveY);

        // Trạng thái nhảy/rơi
        if (collisionChecker.IsGrounded)
        {
            isFalling = false;
        }
        else if (rb.velocity.y > 0 && isJumping)
        {
            isFalling = false;
        }
        else if (rb.velocity.y < 0 && !collisionChecker.IsGrounded)
        {
            isJumping = false;
            isFalling = true;
        }

        Flip(moveX);
        UpdateAnimation(moveX);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;
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