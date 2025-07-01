using System.IO;
using UnityEngine;

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager Instance;

    public int RunCount { get; private set; }

    private string filePath => Application.persistentDataPath + "/session.json";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadRunCount();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementRun()
    {
        RunCount++;
        SaveRunCount();
        Debug.Log($"✅ RunCount: {RunCount}");
    }

    public void ResetRunCount()
    {
        RunCount = 0;
        SaveRunCount();
    }

    private void SaveRunCount()
    {
        SessionData data = new SessionData { runCount = RunCount };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    private void LoadRunCount()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SessionData data = JsonUtility.FromJson<SessionData>(json);
            RunCount = data.runCount;
        }
        else
        {
            RunCount = 0;
            SaveRunCount();
        }
    }
}

[System.Serializable]
public class SessionData
{
    public int runCount;
}
