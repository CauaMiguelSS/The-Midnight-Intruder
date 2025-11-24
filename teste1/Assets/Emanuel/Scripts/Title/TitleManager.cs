using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("CenaPrincipal");
        }
    }
}
