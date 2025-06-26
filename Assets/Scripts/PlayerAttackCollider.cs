using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public float damage = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                Vector2 knockback = other.transform.position - transform.root.position;
                knockback.Normalize();
                damageable.Hit(damage, knockback);
            }
        }
    }
}
