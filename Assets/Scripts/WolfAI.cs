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

    private EnemyBaseState currentState;

    [Header("Movement Settings")]
    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;
    public float wallStuckTime = 0.3f;

    [Header("Detection")]
    public WolfDetector Detector; // Kéo vào Inspector

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
        ChangeState(new PatrolState1(this));
    }

    private void FixedUpdate()
    {
        touchingDirection.Direction = WalkDirectionVector;
        touchingDirection.Check();

        currentState?.FixedUpdate();
    }

    public void ChangeState(EnemyBaseState newState)
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

    public void TriggerAttack()
    {
        animator.SetTrigger("attack");
    }

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);
    public bool LockVelocity => damageable.LockVelocity;
    public CollisionChecker Touching => touchingDirection;
}
