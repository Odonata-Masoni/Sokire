// WolfAI.cs - Main AI Controller using FSM
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CollisionChecker))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Damageable))]
public class WolfAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private CollisionChecker touchingDirection;
    private Animator animator;
    private Damageable damageable;

    private EnemyState currentState;

    [Header("Movement Settings")]
    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;
    public float wallStuckTime = 0.3f;

    public Vector2 WalkDirectionVector { get; private set; } = Vector2.left;
    public float WallTouchTimer { get; set; } = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<CollisionChecker>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();

        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void Start()
    {
        ChangeState(new PatrolState(this));
    }

    private void FixedUpdate()
    {
        touchingDirection.Direction = WalkDirectionVector;
        touchingDirection.Check();

        currentState?.FixedUpdate();
    }

    public void ChangeState(EnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void SetWalkDirection(Vector2 direction)
    {
        WalkDirectionVector = direction;
        float scaleX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(
            direction == Vector2.right ? -scaleX : scaleX,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    public void StopMovement()
    {
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
    }

    public void MoveForward()
    {
        float targetVelocityX = rb.velocity.x + (walkAcceleration * WalkDirectionVector.x * Time.fixedDeltaTime);
        rb.velocity = new Vector2(Mathf.Clamp(targetVelocityX, -maxSpeed, maxSpeed), rb.velocity.y);
    }

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);
    public bool LockVelocity => damageable.LockVelocity;
    public CollisionChecker Touching => touchingDirection;
}

// EnemyState.cs - Abstract base class
public abstract class EnemyState
{
    protected WolfAI wolf;

    public EnemyState(WolfAI wolf) => this.wolf = wolf;

    public virtual void Enter() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}

// PatrolState.cs
public class PatrolState : EnemyState
{
    public PatrolState(WolfAI wolf) : base(wolf) { }

    public override void Enter()
    {
        wolf.SetWalkDirection(Vector2.left);
    }

    public override void FixedUpdate()
    {
        if (wolf.LockVelocity)
            return;

        if (wolf.CanMove)
        {
            wolf.MoveForward();
        }
        else
        {
            wolf.StopMovement();
        }

        if (wolf.Touching.IsTouchingWall && wolf.Touching.IsGrounded)
        {
            wolf.WallTouchTimer += Time.fixedDeltaTime;
            if (wolf.WallTouchTimer >= wolf.wallStuckTime)
            {
                FlipDirection();
                wolf.WallTouchTimer = 0f;
            }
        }
        else
        {
            wolf.WallTouchTimer = 0f;
        }
    }

    private void FlipDirection()
    {
        Vector2 currentDir = wolf.WalkDirectionVector;
        wolf.SetWalkDirection(currentDir == Vector2.right ? Vector2.left : Vector2.right);
    }

    public override void Exit() { }
}
