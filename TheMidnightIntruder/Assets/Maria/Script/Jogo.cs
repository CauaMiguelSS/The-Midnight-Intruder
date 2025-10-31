using UnityEngine;
using UnityEngine.SceneManagement;

public class Jogo : MonoBehaviour
{
    public void Jogar()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Creditos()
    {
        SceneManager.LoadScene("Credits");
    }
    public void CreditosVoltar()
    {
        SceneManager.LoadScene("Maria(Start)");

    }
}
