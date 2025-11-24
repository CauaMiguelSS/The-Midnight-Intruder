using UnityEngine;

public class DialogueDestroyObject : MonoBehaviour
{
    [TextArea(2, 5)]
    public string[] lines;

    [SerializeField] private GameObject objetoParaDestruir; // Porta, muro, etc.
    [SerializeField] private string playerTag = "Player";   // Tag do jogador

    private bool playerEncostando = false;
    private bool hasInteracted = false;

    void Update()
    {
        // Se o player estiver encostando e apertar espaço
        if (playerEncostando && !hasInteracted && Input.GetKeyDown(KeyCode.Space))
        {
            hasInteracted = true;

            // Inicia o diálogo e destrói o objeto no final
            DialogueSystem.Instance.StartDialogue(lines, gameObject.name, false, "", OnDialogueFinished);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            playerEncostando = true;
            Debug.Log("Player encostou no NPC.");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            playerEncostando = false;
            Debug.Log("Player se afastou do NPC.");
        }
    }

    private void OnDialogueFinished()
    {
        if (objetoParaDestruir != null)
        {
            Destroy(objetoParaDestruir);
        }
    }
}






