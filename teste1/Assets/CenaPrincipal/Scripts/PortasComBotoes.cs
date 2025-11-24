using UnityEngine;

public class PortaComBotoes : MonoBehaviour
{
    [Header("Configuração da Porta")]
    public int botoesNecessarios = 2;
    private int botoesAtivos = 0;

    private bool aberta = false;

    public void RegistrarBotaoAtivado()
    {
        if (aberta) return;

        botoesAtivos++;
        if (botoesAtivos >= botoesNecessarios)
        {
            AbrirPorta();
        }
    }

    private void AbrirPorta()
    {
        aberta = true;
        gameObject.SetActive(false);
    }
}

