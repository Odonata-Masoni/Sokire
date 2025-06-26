using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Cài đặt tấn công")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject attackHitbox;

    private void Awake()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }

    // Gọi từ Animation Event lúc vung đòn
    public void EnableAttackHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
            Debug.Log("✅ EnemyAttack hitbox enabled");
        }
    }

    // Gọi từ Animation Event sau khi đánh xong
    public void DisableAttackHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
            Debug.Log("🛑 EnemyAttack hitbox disabled");
        }
    }

    // Cho phép các script khác lấy thông tin damage
    public float GetDamage()
    {
        return damage;
    }
}
