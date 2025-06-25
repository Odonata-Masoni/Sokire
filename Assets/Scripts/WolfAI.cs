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
    public WolfDetector Detector; // Kéo vào trong Inspector từ PlayerDetection

    public Vector2 WalkDirectionVector { get; private set; } = Vector2.left;
    public float WallTouchTimer { get; set; } = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<CollisionChecker>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();

        // Reset scale theo chiều nhìn ban đầu
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void Start()
    {
        ChangeState(new PatrolState1(this));
    }

    private void FixedUpdate()
    {
        // Luôn kiểm tra hướng tiếp xúc để biết có chạm tường, rìa hay không
        touchingDirection.Direction = WalkDirectionVector;
        touchingDirection.Check();

        currentState?.FixedUpdate();
    }
    private void Update()
    {
        currentState?.Update();
    }


    public void ChangeState(EnemyBaseState newState)
    {
        Debug.Log($"[WolfAI] Changing state from {currentState?.GetType().Name} to {newState.GetType().Name}");
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
        Debug.Log($"[WolfAI] New currentState: {currentState?.GetType().Name}");
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
        Debug.Log("TriggerAttack called!");
        animator.ResetTrigger("attack"); // Đảm bảo không bị trigger cũ treo
        animator.SetTrigger("attack");
    }




    public void SetCanMove(bool value)
    {
        animator.SetBool(AnimationStrings.canMove, value);
    }

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);
    public bool LockVelocity => damageable.LockVelocity;
    public CollisionChecker Touching => touchingDirection;

    public void OnAttackAnimationComplete()
    {
        if (currentState is AttackState1 attackState)
        {
            attackState.OnAttackAnimationComplete();
        }
    }
    // Phải là public và không có tham số
    public void EndAttack()
    {
        Debug.Log("🟢 Attack animation ended");
        animator.ResetTrigger("attack");

        // Gọi OnAttackAnimationComplete từ AttackState1
        if (currentState is AttackState1 attackState)
        {
            attackState.OnAttackAnimationComplete();
        }
    }



}
