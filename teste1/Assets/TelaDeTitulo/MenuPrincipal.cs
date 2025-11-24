using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private string nomeDoLevel;
    public void Jogar()
    {
        SceneManager.LoadScene(nomeDoLevel);
    }


    public void Sair()
    {
        Debug.Log("Você saiu");
        Application.Quit();
    }
}
