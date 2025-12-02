using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [Header("Interaction")]
    public float interactionDistance = 3f;
    public KeyCode interactKey = KeyCode.E;
    public string doorText = "Aperte 'E' para destrancar";

    [Header("References")]
    public Camera playerCamera;

    private PlayerItemPickup player;
    private PlayerHighlight playerHighlight;

    public bool alreadyUsed = false;


    void Start()
    {
        player = GetComponent<PlayerItemPickup>();
        playerHighlight = FindObjectOfType<PlayerHighlight>();

        if (playerCamera == null)
            playerCamera = Camera.main;
    }


    void Update()
    {
        if (Input.GetKeyDown(interactKey))
            TryInteract();
    }


    private void TryInteract()
    {
        if (playerCamera == null) return;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position,
                            playerCamera.transform.forward,
                            out hit,
                            interactionDistance))
        {
            LockedDoor door = hit.collider.GetComponent<LockedDoor>();
            if (door == null || player == null) return;

            KeyItem key = player.HeldItem as KeyItem;
            door.TryUnlock(key, player);

            if (!door.IsLocked())
                UnlockDoor();
        }
    }


    public void UnlockDoor()
    {
        alreadyUsed = true;
        Debug.Log("Porta destrancada!");

        if (playerHighlight != null)
            playerHighlight.ForceHideInteraction();
    }

    public string GetMessage()
    {
        return alreadyUsed ? "" : doorText;
    }
}