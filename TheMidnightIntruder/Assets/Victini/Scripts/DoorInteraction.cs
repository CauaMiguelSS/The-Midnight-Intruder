using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public KeyCode interactKey = KeyCode.E;
    private PlayerItemPickup player;

    void Start()
    {
        player = GetComponent<PlayerItemPickup>();
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
            TryInteract();
    }

    void TryInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
        {
            LockedDoor door = hit.collider.GetComponent<LockedDoor>();
            if (door != null && player != null)
            {
                KeyItem key = player.GetComponent<PlayerItemPickup>().GetComponentInChildren<KeyItem>();
                if (key != null)
                    door.TryUnlock(key);
            }
        }
    }
}