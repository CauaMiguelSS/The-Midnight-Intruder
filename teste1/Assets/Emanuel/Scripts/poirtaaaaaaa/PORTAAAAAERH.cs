using UnityEngine;
using UnityEngine.SceneManagement;

public class PORTAAAAERH : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "NomeDaCena";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
