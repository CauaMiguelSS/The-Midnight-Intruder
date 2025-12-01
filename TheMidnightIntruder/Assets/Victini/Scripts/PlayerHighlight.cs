using UnityEngine;
using UnityEngine.UI;

public class PlayerHighlight : MonoBehaviour
{
    [Header("Interaction")]
    public float highlightDistance = 3f;
    public Text interactionText;

    [Header("References")]
    public Camera playerCamera;

    private HighlightController currentItem;
    private DoorInteraction currentDoor;

    // ----------------------------------------------------

    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    // ----------------------------------------------------

    void Update()
    {
        DetectInteractable();
    }

    // ----------------------------------------------------

    private void DetectInteractable()
    {
        if (playerCamera == null) return;

        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position,
                            playerCamera.transform.forward,
                            out hit,
                            highlightDistance))
        {
            DoorInteraction door = hit.collider.GetComponent<DoorInteraction>();
            HighlightController item = hit.collider.GetComponent<HighlightController>();

            if (door != null)
            {
                HandleDoor(door);
                return;
            }

            if (item != null)
            {
                HandleItem(item);
                return;
            }
        }

        ClearAll();
    }

    // ----------------------------------------------------
    // PORTAS
    private void HandleDoor(DoorInteraction door)
    {
        // Desliga item atual
        if (currentItem != null)
        {
            currentItem.Highlight(false);
            currentItem = null;
        }

        currentDoor = door;

        string msg = door.GetMessage();
        ShowText(msg);
    }

    // ----------------------------------------------------
    // ITENS
    private void HandleItem(HighlightController item)
    {
        currentDoor = null;

        if (currentItem != item)
        {
            if (currentItem != null)
                currentItem.Highlight(false);

            currentItem = item;
            currentItem.Highlight(true);
        }

        ShowText("Aperte E para interagir");
    }

    // ----------------------------------------------------

    private void ClearAll()
    {
        if (currentItem != null)
        {
            currentItem.Highlight(false);
            currentItem = null;
        }

        currentDoor = null;
        ShowText("");
    }

    // ----------------------------------------------------

    private void ShowText(string msg)
    {
        if (interactionText == null) return;

        if (string.IsNullOrEmpty(msg))
        {
            interactionText.gameObject.SetActive(false);
            return;
        }

        interactionText.text = msg;
        interactionText.gameObject.SetActive(true);
    }

    // ----------------------------------------------------

    public void ForceHideInteraction()
    {
        ShowText("");
        currentDoor = null;
        currentItem = null;
    }
}