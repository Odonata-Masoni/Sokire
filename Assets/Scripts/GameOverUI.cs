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
    }

    public void Replay()
    {
        PauseMenu.IsGameOver = false;
        Time.timeScale = 1f;

        // ✅ Tính lại số Run
        GameSessionManager.Instance?.IncrementRun();

        // Load lại scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        PauseMenu.IsGameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
