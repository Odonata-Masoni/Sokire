using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string filePath => Application.persistentDataPath + "/save.json";

    public static void SaveGame(Vector2 position, float health)
    {
        SaveData data = new SaveData
        {
            sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            playerX = position.x,
            playerY = position.y,
            health = health
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log("💾 Game saved");
    }

    public static SaveData LoadGame()
    {
        if (!File.Exists(filePath)) return null;

        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static bool HasSave() => File.Exists(filePath);

    public static void DeleteSave()
    {
        if (File.Exists(filePath)) File.Delete(filePath);
    }
}
