using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public Transform interactionPoint;
    public float interactionRadius = 0.5f;
    public LayerMask interactableLayer;

    void Update()
    {
        if (DialogueSystem.Instance != null && DialogueSystem.Instance.DialogueAtivo())
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider2D hit = Physics2D.OverlapCircle(interactionPoint.position,interactionRadius,interactableLayer);

            if (hit != null)
            {
                DialogueTrigger trigger = hit.GetComponent<DialogueTrigger>();
                if (trigger != null)
                {
                    trigger.Interact();
                    return;
                }

                DialogueTriggerSolo triggerSolo = hit.GetComponent<DialogueTriggerSolo>();
                if (triggerSolo != null)
                {
                    triggerSolo.Interact();
                    return;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (interactionPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
        }
    }
}
