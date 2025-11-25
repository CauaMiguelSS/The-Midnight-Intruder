using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManeger : MonoBehaviour
{
    public void TrocaDeCenaControl()
    {
        SceneManager.LoadScene("Maria(Start)");
        Time.timeScale = 1f;
    }
}
