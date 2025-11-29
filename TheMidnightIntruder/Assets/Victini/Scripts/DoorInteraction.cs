using UnityEngine;
 
public class DoorInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public KeyCode interactKey = KeyCode.E;
    public Camera playerCamera;
 
    private PlayerItemPickup player;
 
    void Start()
    {
        player = GetComponent<PlayerItemPickup>();
       
        if (playerCamera == null)
            playerCamera = Camera.main;
    }
 
    void Update()
    {
        if (Input.GetKeyDown(interactKey))
            TryInteract();
    }
 
    void TryInteract()
    {
        RaycastHit hit;
       
        if (playerCamera == null) return;
       
        if (Physics.Raycast(playerCamera.transform.position,
                            playerCamera.transform.forward,
                            out hit,
                            interactionDistance))
        {
            LockedDoor door = hit.collider.GetComponent<LockedDoor>();
           
            if (door != null && player != null)
            {
                 KeyItem key = player.HeldItem as KeyItem; 
                 door.TryUnlock(key, player);
            }
        }
    }
}