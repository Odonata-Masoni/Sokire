using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class ExecutionerAI : MonoBehaviour
{
    [Header("Di chuyển")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDistance = 5f;

    [Header("Tấn công")]
    public bool PlayerInAttackZone = false;
    private bool isAttacking = false;

    [SerializeField] private Collider2D attackHitbox;
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private Vector2 knockback = new Vector2(3f, 1f);

    private bool isMoving = false;
    private bool canMove = true;
    private float direction = 1f;
    private float startPosition;
    private Damageable damageable;

    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        startPosition = transform.position.x;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
    }

    void FixedUpdate()
    {
        // Dừng mọi thứ nếu không còn sống
        if (damageable != null && !damageable.IsAlive)
        {
            rb.velocity = Vector2.zero;
            isMoving = false;
            return;
        }

        if (PlayerInAttackZone && !isAttacking)
        {
            StartAttack();
        }
        else if (canMove && !isAttacking)
        {
            Move();
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            isMoving = false;
        }

        UpdateAnimator();
    }


    private void Move()
    {
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        isMoving = true;

        float distanceFromStart = Mathf.Abs(transform.position.x - startPosition);
        if (distanceFromStart >= moveDistance)
        {
            Flip();
            direction *= -1;

            // Cập nhật lại vị trí bắt đầu mới để tránh flip liên tục
            startPosition = transform.position.x;
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void UpdateAnimator()
    {
        animator.SetBool("isMoving", isMoving);
    }

    private void StartAttack()
    {
        isAttacking = true;
        isMoving = false;
        rb.velocity = Vector2.zero; // Dừng lại khi tấn công
        animator.SetTrigger("attack");
    }

    // Gọi từ Animation Event cuối đòn tấn công
    public void EndAttack()
    {
        isAttacking = false;
    }

    // Gọi từ Animation Event tại frame gây damage
    public void DealDamage()
    {
        if (attackHitbox == null) return;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            attackHitbox.bounds.center,
            attackHitbox.bounds.size,
            0f
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Damageable dmg = hit.GetComponent<Damageable>();
                if (dmg != null)
                {
                    Vector2 knockDir = (hit.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right;
                    dmg.Hit(attackDamage, knockback * knockDir);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackHitbox != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackHitbox.bounds.center, attackHitbox.bounds.size);
        }
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!canMove)
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
