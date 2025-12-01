using UnityEngine;
using UnityEngine.SceneManagement;

public class UIChange : MonoBehaviour
{
    [SerializeField] private string sceneName;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChangeScene();
        }
    }

    public void ChangeScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}


