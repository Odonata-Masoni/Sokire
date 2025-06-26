using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;

    private float lastAttackTime = 0f;

    private Animator animator;
    private CollisionChecker collisionChecker;

    [Header("Tấn công")]
    [SerializeField] private GameObject attackHitbox;  // 👈 Kéo object AttackHitbox vào đây từ Inspector
    [SerializeField] public float damage = 10f;
    void Awake()
    {
        animator = GetComponent<Animator>();
        collisionChecker = GetComponent<CollisionChecker>();

        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false); // Tắt sẵn khi khởi động

            // Truyền Attack vào PlayerAttackHitbox
            PlayerAttackHitbox hitbox = attackHitbox.GetComponent<PlayerAttackHitbox>();
            if (hitbox != null)
            {
                hitbox.SetAttack(this);
            }
            else
            {
                Debug.LogWarning("❌ Không tìm thấy PlayerAttackHitbox trên attackHitbox!");
            }
        }
    }


    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (Time.time < lastAttackTime + attackCooldown) return;

        if (!collisionChecker.IsGrounded && !collisionChecker.IsTouchingWall) return;

        lastAttackTime = Time.time;
        animator.SetTrigger(AnimationStrings.isAttacking);
    }

    // Animation Event sẽ gọi các hàm này đúng thời điểm vung đòn
    public void EnableAttackHitbox()
    {
        Debug.Log("✅ EnableAttackHitbox() CALLED!");
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
            Debug.Log("✅ Attack hitbox enabled");
        }
        else
        {
            Debug.LogWarning("❌ AttackHitbox is null");
        }
    }



    public void DisableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }
}
