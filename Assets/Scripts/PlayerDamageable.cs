using UnityEngine;

public class PlayerDamageable : MonoBehaviour
{
    private Damageable damageable;

    [SerializeField] private GameOverUI gameOverUI;

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
        Debug.Log("🟥 Player bị đánh: " + dmg);
        // TODO: Play animation, UI flash, shake camera...
    }
    public void OnDeathAnimationFinished()
    {
        if (gameOverUI != null)
        {
            gameOverUI.ShowGameOver();
        }
        else
        {
            Debug.LogWarning("⚠️ GameOverUI chưa được gán!");
        }
    }

    private void OnDeath()
    {
        Debug.Log("☠️ Player chết");

        if (gameOverUI != null)
        {
            gameOverUI.ShowGameOver();
        }
        else
        {
            Debug.LogWarning("⚠️ Chưa gán GameOverUI vào PlayerDamageable");
        }
    }
}
