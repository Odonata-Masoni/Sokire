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
    [SerializeField] private float attackCooldown = 1.2f;
    private float lastAttackTime = -999f;

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
        if (damageable != null && !damageable.IsAlive)
        {
            rb.velocity = Vector2.zero;
            isMoving = false;
            return;
        }

        if (PlayerInAttackZone && !isAttacking && Time.time - lastAttackTime >= attackCooldown)
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
        rb.velocity = Vector2.zero;
        lastAttackTime = Time.time;
        animator.SetTrigger("attack");
        Debug.Log("⚔️ Executioner bắt đầu tấn công!");
    }

    public void EndAttack()
    {
        isAttacking = false;
        Debug.Log("⏹ Kết thúc đòn tấn công");
    }

    public void DealDamage()
    {
        if (attackHitbox == null)
        {
            Debug.LogWarning("⚠️ attackHitbox chưa được gán.");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            attackHitbox.bounds.center,
            attackHitbox.bounds.size,
            0f
        );

        Debug.Log($"🟥 Số đối tượng bị trúng đòn: {hits.Length}");

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                var dmg = hit.GetComponent<Damageable>();
                if (dmg != null)
                {
                    Vector2 knockDir = (hit.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right;
                    dmg.Hit(attackDamage, knockback * knockDir);
                    Debug.Log("🎯 Player đã bị trúng đòn");
                }
            }
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    if (attackHitbox != null)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireCube(attackHitbox.bounds.center, attackHitbox.bounds.size);
    //    }
    //}

    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!value)
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
        }
    }

    public bool IsMoving() => isMoving;
}
