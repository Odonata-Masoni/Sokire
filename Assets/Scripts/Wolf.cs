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

    private WalkableDirection _walkDirection = WalkableDirection.Left; // Sprite mặc định nhìn trái
    private Vector2 walkDirectionVector = Vector2.left;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            _walkDirection = value;

            // Cập nhật hướng di chuyển
            walkDirectionVector = (_walkDirection == WalkableDirection.Right) ? Vector2.right : Vector2.left;

            // Lật sprite bằng cách đặt đúng scale.x theo hướng
            float scaleX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector2(
                (_walkDirection == WalkableDirection.Right) ? -scaleX : scaleX,
                transform.localScale.y
            );
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
    }

    private void Start()
    {
        WalkDirection = WalkableDirection.Left;
    }

    private void FixedUpdate()
    {
        // Cập nhật hướng để CollisionChecker raycast đúng
        touchingDirection.WalkDirectionVector = walkDirectionVector;

        touchingDirection.CheckCollisions();

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

            Debug.Log("CanMove: " + CanMove + " | IsGrounded: " + touchingDirection.IsGrounded +
                      " | IsTouchingWall: " + touchingDirection.IsTouchingWall + " | Velocity: " + rb.velocity);
        }

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
