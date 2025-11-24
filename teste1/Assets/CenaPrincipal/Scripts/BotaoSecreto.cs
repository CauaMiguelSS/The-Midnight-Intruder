using UnityEngine;

public class BotaoSecreto : MonoBehaviour
{
    [Header("Portas controladas por este botão")]
    public PortaComBotoes[] portas;

    private bool jogadorPerto = false;
    private bool ativado = false;

    void Update()
    {
        if (jogadorPerto && !ativado && Input.GetKeyDown(KeyCode.E))
        {
            ativado = true;

            foreach (var p in portas)
            {
                if (p != null)
                    p.RegistrarBotaoAtivado();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jogadorPerto = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jogadorPerto = false;
        }
    }
}
