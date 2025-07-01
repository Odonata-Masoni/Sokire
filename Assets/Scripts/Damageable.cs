using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    public float currentHealth;

    [Header("Invincibility")]
    [SerializeField] private float invincibilityTime = 0.25f;
    private bool isInvincible = false;
    private float timeSinceHit = 0f;

    [Header("Events")]
    public UnityEvent<float, Vector2> OnTakeDamage;
    public UnityEvent OnDie;

    protected Animator animator;

    private bool isAlive = true;
    public bool IsAlive
    {
        get => isAlive;
        private set
        {
            if (isAlive == value) return;
            isAlive = value;

            if (animator != null)
            {
                animator.SetBool(AnimationStrings.isAlive, value);
                animator.SetBool(AnimationStrings.canMove, false);
                animator.SetBool(AnimationStrings.lockVelocity, true);
            }

            Debug.Log("IsAlive set to: " + value);

        }
    }

    public bool LockVelocity
    {
        get => animator.GetBool(AnimationStrings.lockVelocity);
        set => animator.SetBool(AnimationStrings.lockVelocity, value);
    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        isAlive = true;
    }

    protected virtual void Update()
    {
        if (isInvincible)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0f;
            }
        }
    }

    public virtual void Hit(float damage, Vector2 knockback)
    {
        if (!IsAlive || isInvincible) return;

        currentHealth -= damage;
        Debug.Log($"🟥 {gameObject.name} took {damage} damage!");

        OnTakeDamage?.Invoke(damage, knockback);

        if (currentHealth <= 0)
        {
            IsAlive = false;
        }

        animator.SetTrigger(AnimationStrings.hitTrigger);
        LockVelocity = true;
        isInvincible = true;
        timeSinceHit = 0;

        if (IsAlive)
        {
            StartCoroutine(UnlockVelocityAfterDelay(0.55f));
        }
    }

    protected virtual System.Collections.IEnumerator UnlockVelocityAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!IsAlive) yield break;

        LockVelocity = false;
        animator.SetBool(AnimationStrings.canMove, true);

        if (TryGetComponent<WolfAI>(out var wolf))
        {
            wolf.OnAttackAnimationComplete();
        }
    }

    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        IsAlive = true;
        isInvincible = false;
        timeSinceHit = 0f;
    }
    public void SetCurrentHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0, maxHealth);
        IsAlive = currentHealth > 0;
    }

}
