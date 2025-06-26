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

    void Awake()
    {
        animator = GetComponent<Animator>();
        collisionChecker = GetComponent<CollisionChecker>();
        if (attackHitbox != null)
            attackHitbox.SetActive(false); // Tắt sẵn khi khởi động
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
        if (attackHitbox != null)
            attackHitbox.SetActive(true);
    }

    public void DisableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }
}
