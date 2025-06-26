using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    private EnemyAttack enemyAttack;

    private void Awake()
    {
        enemyAttack = GetComponentInParent<EnemyAttack>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                Vector2 knockback = other.transform.position - transform.root.position;
                knockback.Normalize();
                float damage = enemyAttack != null ? enemyAttack.GetDamage() : 10f;

                damageable.Hit(damage, knockback);
                Debug.Log($"🗡️ Enemy hit Player with {damage} damage");
            }
        }
    }
}
