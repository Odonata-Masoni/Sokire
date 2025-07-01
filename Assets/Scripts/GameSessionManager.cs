using UnityEngine;

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager Instance { get; private set; }

    public int RunCount { get; private set; }

    private void Awake()
    {
        // Singleton pattern + lưu lại giữa các scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Lấy RunCount từ lần trước (nếu có)
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
