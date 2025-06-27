using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<float, Vector2> damageableHit;    // Cho Skeleton.OnHit
    public UnityEvent<float, Vector2, bool> damageableHitP;   // Cho PlayerController.OnHitP
    public UnityEvent damageableDeath;

    Animator animator;

    [SerializeField] private float _maxHealth = 100f;
    public float MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    [SerializeField] private float _health = 100f;
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            Debug.Log($"❤️ Health updated to: {_health} / {MaxHealth}");
            if (_health <= 0 && IsAlive)
            {
                _health = 0;
                IsAlive = false; // Gọi logic chết duy nhất ở đây
                Debug.Log($"☠️ Entity died!");
            }
        }
    }

    [SerializeField] private bool _isAlive = true;
    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            if (!_isAlive && !value) return; // ⚠️ Nếu đã chết rồi thì không xử lý lại

            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set to: " + value);

            if (!value)
            {
                damageableDeath.Invoke();
            }
        }
    }

    public bool LockVelocity
    {
        get => animator.GetBool(AnimationStrings.lockVelocity);
        set => animator.SetBool(AnimationStrings.lockVelocity, value);
    }

    private bool isInvincible = false;
    private float timeSinceHit = 0f;
    public float isInvincibilityTime = 0.25f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        damageableHit ??= new UnityEvent<float, Vector2>();
        damageableHitP ??= new UnityEvent<float, Vector2, bool>();
        Health = MaxHealth;
    }

    private void Update()
    {
        if (isInvincible)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit > isInvincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
        }
    }

    public void Hit(float damage, Vector2 knockback)
    {
        Debug.Log($"🟥 Damageable.Hit called! Damage: {damage}");

        if (!IsAlive || isInvincible)
            return;

        Health -= damage;

        animator.SetTrigger(AnimationStrings.hitTrigger);
        LockVelocity = true;
        Debug.Log("🔒 LockVelocity set to TRUE");

        isInvincible = true;
        timeSinceHit = 0;

        damageableHit?.Invoke(damage, knockback);
        damageableHitP?.Invoke(damage, knockback, LockVelocity);

        if (IsAlive) // Nếu chưa chết, thì mới unlock sau delay
        {
            StartCoroutine(UnlockVelocityAfterDelay(0.55f));
        }
    }

    private IEnumerator UnlockVelocityAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!IsAlive)
        {
            Debug.Log("🔕 UnlockVelocity canceled (dead)");
            yield break;
        }

        LockVelocity = false;
        animator.SetBool(AnimationStrings.canMove, true);
        Debug.Log("🟢 LockVelocity set to FALSE, canMove = true");

        if (TryGetComponent<WolfAI>(out var wolf))
        {
            wolf.OnAttackAnimationComplete(); // Gọi kết thúc animation nếu còn sống
        }
    }
}
