using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        PauseMenu.IsGameOver = true;
        Time.timeScale = 0f;

        // ❌ Xóa file JSON khi chết để không còn nút Continue
        SaveSystem.DeleteSave();
    }

    public void Replay()
    {
        PauseMenu.IsGameOver = false;
        Time.timeScale = 1f;

        GameSessionManager.Instance?.IncrementRun();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        PauseMenu.IsGameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
