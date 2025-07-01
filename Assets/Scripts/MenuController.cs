using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        GameSessionManager.Instance?.IncrementRun(); // ✅ Tăng tại đây

        SceneManager.LoadScene("GamePlay Scene 1"); // Đổi đúng tên scene chơi
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
