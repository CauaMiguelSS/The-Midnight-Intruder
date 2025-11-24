using UnityEngine;

public class DestruirObjeto : MonoBehaviour
{
    public AtivarBotao botao1;
    public AtivarBotao botao2;

    public AudioClip somPorta; // <-- arrasta o áudio aqui!
    private bool tocouSom = false;

    void Update()
    {
        if (botao1.estaPressionado && botao2.estaPressionado)
        {
            if (!tocouSom)
            {
                AudioSource.PlayClipAtPoint(somPorta, transform.position, 1f);
                tocouSom = true;
            }

            gameObject.SetActive(false); // desativa a porta
        }
        else
        {
            // Se a porta voltar a ativar, permite tocar de novo quando desativar
            tocouSom = false;
            gameObject.SetActive(true);
        }
    }
}
