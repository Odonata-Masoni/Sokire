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
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    [SerializeField] private float _health = 100f;
    public float Health
    {
        get { return _health; }
        set
        {
            _health = value;
            Debug.Log($"Health updated to: {_health} / {MaxHealth}");
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }
    [SerializeField] private bool _isAlive = true;
    private bool isInvincible = false;

    private float timeSinceHit = 0;
    public float isInvincibilityTime = 0.25f;

    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set to: " + value);

            if (value == false)
            {
                damageableDeath.Invoke();
            }
        }
    }
    public bool LockVelocity
    {
        get { return animator.GetBool(AnimationStrings.lockVelocity); }
        set { animator.SetBool(AnimationStrings.lockVelocity, value); }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (damageableHit == null)
        {
            damageableHit = new UnityEvent<float, Vector2>();
        }
        if (damageableHitP == null)
        {
            damageableHitP = new UnityEvent<float, Vector2, bool>();
        }
        Health = MaxHealth; // Khởi tạo Health từ MaxHealth
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > isInvincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }

    public void Hit(float damage, Vector2 knockback)
    {
        Debug.Log($"🟥 Damageable.Hit called! Damage: {damage}");

        if (!IsAlive)
            return;

        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            IsAlive = false;
            Debug.Log($"🟥 Enemy died!");
            // Death logic...
        }

        Debug.Log($"🟥 Health updated to: {Health} / {MaxHealth}");
    }
    //public bool Heal(float healthRestore)
    //{
    //    if (IsAlive && Health < MaxHealth)
    //    {
    //        float maxHeal = Mathf.Max(MaxHealth - Health, 0);
    //        float actualHeal = Mathf.Min(maxHeal, healthRestore);
    //        Health += actualHeal;
    //        CharacterEvents.characterHealed(gameObject, actualHeal);
    //        return true;
    //    }

    //    return false;
    //}
}