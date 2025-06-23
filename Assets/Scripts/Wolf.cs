using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;

    Rigidbody2D rb;
    CollisionChecker touchingDirection;
    Animator animator;
    Damageable damageable;

    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection = WalkableDirection.Right; // Khởi tạo mặc định là Right
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }

    public float AttackCooldown
    {
        get { return animator.GetFloat(AnimationStrings.attackCD); }
        private set { animator.SetFloat(AnimationStrings.attackCD, Mathf.Max(value, 0)); }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<CollisionChecker>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        // Khởi động di chuyển mặc định
        WalkDirection = WalkableDirection.Right; // Bắt đầu đi sang phải
    }

    private void FixedUpdate()
    {
        touchingDirection.CheckCollisions();
        if (!damageable.LockVelocity)
        {
            if (CanMove)
            {
                // Áp dụng gia tốc di chuyển
                float targetVelocityX = rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime);
                rb.velocity = new Vector2(
                    Mathf.Clamp(targetVelocityX, -maxSpeed, maxSpeed), rb.velocity.y);
            }
            else
            {
                // Giảm tốc độ khi dừng
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
            Debug.Log("CanMove: " + CanMove + " | IsGrounded: " + touchingDirection.IsGrounded +
                      " | IsTouchingWall: " + touchingDirection.IsTouchingWall + " | Velocity: " + rb.velocity);
        }

        // Kiểm tra và lật hướng khi chạm tường
        if (touchingDirection.IsTouchingWall)
        {
            FlipDirection();
        }
    }

    private void FlipDirection()
    {
        WalkDirection = (WalkDirection == WalkableDirection.Right) ? WalkableDirection.Left : WalkableDirection.Right;
        Debug.Log("Flipped to: " + WalkDirection);
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