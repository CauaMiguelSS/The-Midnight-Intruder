using UnityEngine;

public class AtivarBotao : MonoBehaviour
{
    public bool estaPressionado = false;
    private int objetosNoBotao = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Caixa") || collision.CompareTag("Player"))
        {
            objetosNoBotao++;
            AtualizarEstado();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Caixa") || collision.CompareTag("Player"))
        {
            objetosNoBotao--;
            AtualizarEstado();
        }
    }

    private void AtualizarEstado()
    {
        if (objetosNoBotao > 0)
        {
            estaPressionado = true;
            Debug.Log("Botão Ativado");
        }
        else
        {
            estaPressionado = false;
            Debug.Log("Botão Inativo");
        }
    }
}


