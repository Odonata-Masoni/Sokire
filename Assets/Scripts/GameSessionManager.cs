using UnityEngine;

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager Instance { get; private set; }

    public int RunCount { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

#if !UNITY_EDITOR
    // 👉 Khi không chạy trong Unity Editor (tức là bản build)
    if (!PlayerPrefs.HasKey("RunCount"))
    {
        PlayerPrefs.SetInt("RunCount", 0); // hoặc dùng DeleteAll() nếu cần reset toàn bộ
        PlayerPrefs.Save();
        Debug.Log("🧼 Reset RunCount về 0 khi build lần đầu");
    }
#endif

        RunCount = PlayerPrefs.GetInt("RunCount", 0);
    }


    public void IncrementRun()
    {
        RunCount++;
        PlayerPrefs.SetInt("RunCount", RunCount);
        PlayerPrefs.Save();
        Debug.Log("🔁 RunCount = " + RunCount);
    }
}
