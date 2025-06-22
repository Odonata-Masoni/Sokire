using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    private Animator animator;
    private CollisionChecker collisionChecker;

    void Awake()
    {
        animator = GetComponent<Animator>();
        collisionChecker = GetComponent<CollisionChecker>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // Nếu đang trong thời gian hồi chiêu thì không cho tấn công
        if (Time.time < lastAttackTime + attackCooldown) return;

        // Tùy chọn: Chỉ tấn công khi đang trên mặt đất
        if (!collisionChecker.IsGrounded && !collisionChecker.IsTouchingWall) return;

        lastAttackTime = Time.time;
        isAttacking = true;

        // Bật animation
        animator.SetTrigger(AnimationStrings.isAttacking);
    }

    void Update()
    {
        // Tuỳ chọn reset sau attack nếu cần
        if (isAttacking && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            // vẫn đang trong animation attack
        }
        else
        {
            isAttacking = false;
        }
    }
}
