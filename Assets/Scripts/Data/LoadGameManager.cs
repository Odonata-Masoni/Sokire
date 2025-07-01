using UnityEngine;

public class LoadGameManager : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        // Không phải từ "Continue" thì bỏ qua
        if (!PlayerPrefs.HasKey("sceneToLoad")) return;

        SaveData data = SaveSystem.LoadGame();
        if (data == null) return;

        // 🔁 Đặt lại vị trí
        player.transform.position = new Vector2(data.playerX, data.playerY);

        // 🔁 Set lại máu
        var damageable = player.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.SetCurrentHealth(data.health); // cần thêm hàm này nếu chưa có
        }

        // Xoá cờ để không load lại lần nữa
        PlayerPrefs.DeleteKey("sceneToLoad");
    }
}
