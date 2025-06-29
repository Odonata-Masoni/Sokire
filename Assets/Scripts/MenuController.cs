using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GamePlay Scene 1"); // Đổi tên thành đúng scene bạn dùng
    }

    public void QuitGame()
    {
        Debug.Log("Exit button pressed - Application.Quit() called");
        Application.Quit();
    }
}
