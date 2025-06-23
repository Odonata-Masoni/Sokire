using UnityEngine;

public class Wolf : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;
    public float wallStuckTime = 0.3f;

    private Rigidbody2D rb;
    private CollisionChecker touchingDirection;
    private Animator animator;
    private Damageable damageable;

    public enum WalkableDirection { Right, Left }

    private WalkableDirection _walkDirection = WalkableDirection.Left;
    private Vector2 walkDirectionVector = Vector2.left;
    private float wallTouchTimer = 0f;

    public WalkableDirection WalkDirection
    {
        get => _walkDirection;
        set
        {
            _walkDirection = value;
            walkDirectionVector = (value == WalkableDirection.Right) ? Vector2.right : Vector2.left;
            UpdateSpriteFlip();
        }
    }

    private void UpdateSpriteFlip()
    {
        float scaleX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(
            (_walkDirection == WalkableDirection.Right) ? -scaleX : scaleX,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    public bool HasTarget
    {
        get => animator.GetBool(AnimationStrings.hasTarget);
        private set => animator.SetBool(AnimationStrings.hasTarget, value);
    }

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);

    public float AttackCooldown
    {
        get => animator.GetFloat(AnimationStrings.attackCD);
        private set => animator.SetFloat(AnimationStrings.attackCD, Mathf.Max(value, 0));
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<CollisionChecker>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();

        // Chuẩn hóa scale ngay từ đầu (quan trọng)
        float scaleX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Start()
    {
        WalkDirection = WalkableDirection.Left;
    }

    private void FixedUpdate()
    {
        touchingDirection.Direction = walkDirectionVector;
        touchingDirection.Check();

        if (!damageable.LockVelocity)
        {
            if (CanMove)
            {
                float targetVelocityX = rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime);
                rb.velocity = new Vector2(Mathf.Clamp(targetVelocityX, -maxSpeed, maxSpeed), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }

        if (touchingDirection.IsTouchingWall && touchingDirection.IsGrounded)
        {
            wallTouchTimer += Time.fixedDeltaTime;
            if (wallTouchTimer >= wallStuckTime)
            {
                FlipDirection();
                wallTouchTimer = 0f;
            }
        }
        else
        {
            wallTouchTimer = 0f;
        }
    }

    private void FlipDirection()
    {
        WalkDirection = (WalkDirection == WalkableDirection.Right) ? WalkableDirection.Left : WalkableDirection.Right;
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirection.IsGrounded)
        {
            FlipDirection();
        }
    }
}
