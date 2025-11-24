using UnityEngine;

public class TextoTrigger : MonoBehaviour
{
    [TextArea(2, 5)]
    public string mensagem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TextoComFadeManager.Instance.MostrarTexto(mensagem);
        }
    }
}

