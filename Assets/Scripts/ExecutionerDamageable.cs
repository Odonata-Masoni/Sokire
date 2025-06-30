using UnityEngine;

public class ExecutionerDamageable : MonoBehaviour
{
    private Damageable damageable;
    private ExecutionerAI executionerAI;

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        executionerAI = GetComponent<ExecutionerAI>();
    }

    private void OnEnable()
    {
        damageable.OnTakeDamage.AddListener(OnHit);
        damageable.OnDie.AddListener(OnDeath);
    }

    private void OnDisable()
    {
        damageable.OnTakeDamage.RemoveListener(OnHit);
        damageable.OnDie.RemoveListener(OnDeath);
    }

    private void OnHit(float dmg, Vector2 knockback)
    {
        Debug.Log("🟥 Executioner bị đánh: " + dmg);
        // TODO: hiệu ứng máu, hiệu ứng flicker, âm thanh
    }

    private void OnDeath()
    {
        Debug.Log("☠️ Executioner chết");

        if (executionerAI != null)
        {
            executionerAI.SetCanMove(false);
            executionerAI.enabled = false;
        }

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("die");
        }

        // TODO: Xoá sau delay, play sound, drop item, v.v.
        // Destroy(gameObject, 3f);
    }
}
