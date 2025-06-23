using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Cấu hình di chuyển")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;

    [Header("Cấu hình nhảy")]
    public float jumpHeight = 4f;
    public float jumpApexTime = 0.35f;

    [Header("Cấu hình va chạm")]
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private CollisionChecker collisionChecker;

    private Vector2 input;
    private float gravity;
    private float jumpVelocity;
    private float facing = 1f;

    private bool isRunning = false;
    private bool isJumping = false;
    private bool isFalling = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collisionChecker = GetComponent<CollisionChecker>();

        gravity = -(2 * jumpHeight) / (jumpApexTime * jumpApexTime);
        jumpVelocity = Mathf.Abs(gravity) * jumpApexTime;
        rb.gravityScale = 0f;

        collisionChecker.Direction = Vector2.right;
    }

    private void FixedUpdate()
    {
        collisionChecker.Check();

        float horizontal = animator.GetBool(AnimationStrings.canMove) ? input.x : 0f;
        float vertical = rb.velocity.y;

        if (collisionChecker.IsTouchingCeiling && vertical > 0)
        {
            vertical = 0f;
        }

        // Apply gravity if not grounded
        if (!collisionChecker.IsGrounded)
        {
            vertical += gravity * Time.fixedDeltaTime;
        }

        // Nếu chạm tường trên không, dừng trượt
        if (collisionChecker.IsTouchingWall && !collisionChecker.IsGrounded)
        {
            horizontal = 0f;
        }

        float speed = isRunning ? runSpeed : walkSpeed;
        rb.velocity = new Vector2(horizontal * speed, Mathf.Clamp(vertical, -20f, 20f));

        UpdateJumpFallState(vertical);
        FlipCharacter(horizontal);
        collisionChecker.Direction = new Vector2(facing, 0);

        UpdateAnimator(horizontal);
    }

    private void UpdateJumpFallState(float velocityY)
    {
        if (collisionChecker.IsGrounded)
        {
            isJumping = false;
            isFalling = false;
        }
        else if (velocityY > 0)
        {
            isJumping = true;
            isFalling = false;
        }
        else if (velocityY < 0)
        {
            isJumping = false;
            isFalling = true;
        }
    }

    private void FlipCharacter(float horizontal)
    {
        if (horizontal > 0.1f && facing < 0)
        {
            facing = 1f;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
        else if (horizontal < -0.1f && facing > 0)
        {
            facing = -1f;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
    }

    private void UpdateAnimator(float horizontal)
    {
        animator.SetBool(AnimationStrings.isMoving, Mathf.Abs(horizontal) > 0.1f);
        animator.SetBool(AnimationStrings.isRunning, isRunning);
        animator.SetBool(AnimationStrings.isJumping, isJumping);
        animator.SetBool(AnimationStrings.isFalling, isFalling);
        animator.SetBool(AnimationStrings.isGrounded, collisionChecker.IsGrounded);
    }

    // Input Events (có thể gọi từ Unity InputSystem)
    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>().normalized;
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
}
