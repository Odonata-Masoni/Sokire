using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{

    
    private Attack attack;

    public void SetAttack(Attack atk)
    {
        attack = atk;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Hitbox triggered with: {other.name}");

        // Lấy GameObject gốc (root hoặc có chứa Damageable)
        Damageable damageable = other.GetComponentInParent<Damageable>();
        if (damageable != null)
        {
            Vector2 knockback = other.transform.position - transform.root.position;
            knockback.Normalize();
            damageable.Hit(attack.damage, knockback);
            Debug.Log($"✅ Gây damage {attack.damage} lên: {other.name}");
        }
        else
        {
            Debug.Log($"❌ Không tìm thấy Damageable trong {other.name} hoặc cha của nó");
        }
    }
}
