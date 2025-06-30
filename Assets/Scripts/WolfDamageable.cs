using UnityEngine;

public class WolfDamageable : MonoBehaviour
{
    private Damageable damageable;

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
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
        Debug.Log("🟥 Wolf bị đánh: " + dmg);
        // TODO: Play blood FX, flicker sprite, etc.
    }

    private void OnDeath()
    {
        Debug.Log("☠️ Wolf chết");
       
        // TODO: Drop item, play death animation, sound, etc.
    }
}
