using UnityEngine;
using UnityEngine.SceneManagement;

public class BotaoTrocaCena : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private bool sairDoJogo = false;   
    [SerializeField] private string nomeCenaDestino = "";  

    public void ExecutarAcao()
    {
        if (sairDoJogo)
        {
            SairDoJogo();
        }
        else if (!string.IsNullOrEmpty(nomeCenaDestino))
        {
            TrocarCena();
        }
    }

    private void TrocarCena()
    {
        SceneManager.LoadScene(nomeCenaDestino);
    }

    private void SairDoJogo()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}


