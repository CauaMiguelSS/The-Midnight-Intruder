using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitBtn : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Maria(Start)");
    }

    public void RestartGame()
    {
        PlayerLock.IsLocked = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}