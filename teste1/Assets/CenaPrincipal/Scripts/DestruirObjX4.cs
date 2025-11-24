using UnityEngine;

public class DestruirObjX4 : MonoBehaviour
{
    public AtivarBotao botao5;
    public AtivarBotao botao6;
    public AtivarBotao botao3;
    public AtivarBotao botao4;

    public AudioSource audioSource;   // arrasta o AudioSource aqui no Inspector
    private bool jaTocouSom = false;  // impede tocar várias vezes

    void Update()
    {
        bool todosPressionados = botao6.estaPressionado &&
                                 botao5.estaPressionado &&
                                 botao3.estaPressionado &&
                                 botao4.estaPressionado;

        if (todosPressionados)
        {
            if (!jaTocouSom)
            {
                if (audioSource != null)
                    audioSource.Play();

                jaTocouSom = true;
            }

            gameObject.SetActive(false); // Destrói/desativa o portão
        }
        else
        {
            gameObject.SetActive(true);
            jaTocouSom = false; // permite tocar de novo quando fechar e depois abrir
        }
    }
}
