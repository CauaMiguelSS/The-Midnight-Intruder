using UnityEngine;

public class DialogueTriggerSolo : MonoBehaviour
{
    [TextArea(2, 5)]
    public string[] falasSolo;

    public GameObject objetoParaDestruir;

    public void Interact()
    {
        DialogueSystem.Instance.StartDialogue(
            falasSolo,
            gameObject.name,
            false,
            "",
            AoTerminarDialogo
        );
    }

    private void AoTerminarDialogo()
    {
        if (objetoParaDestruir != null)
        {
            Destroy(objetoParaDestruir);
            objetoParaDestruir = null;
        }
    }
}




