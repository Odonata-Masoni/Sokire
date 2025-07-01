using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject continueButton;

    private void Start()
    {
        // ✅ Kiểm tra xem có file save không
        continueButton.SetActive(SaveSystem.HasSave());
    }

    public void StartGame()
    {
        GameSessionManager.Instance?.IncrementRun();
        SceneManager.LoadScene("GamePlay Scene 1"); // sửa đúng tên scene
    }

    public void ContinueGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data != null)
        {
            PlayerPrefs.SetString("sceneToLoad", data.sceneName);
            SceneManager.LoadScene(data.sceneName);            // hoặc data.sceneName nếu không dùng scene trung gian
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
