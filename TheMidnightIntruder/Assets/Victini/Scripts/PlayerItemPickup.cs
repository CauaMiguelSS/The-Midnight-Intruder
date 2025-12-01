using UnityEngine;
 
public class PlayerItemPickup : MonoBehaviour
{
    public float pickupDistance = 3f;
    public Transform holdPoint;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;
    public Camera playerCamera;
 
    public Item heldItem;
    public Item HeldItem => heldItem; // Permite o acesso seguro ao item segurado.
 
    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
            TryPickupItem();
 
        if (Input.GetKeyDown(dropKey))
            DropItem();
    }
 
    void TryPickupItem()
    {
        if (heldItem != null) return;
 
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position,
                            playerCamera.transform.forward,
                            out hit,
                            pickupDistance))
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
                Pickup(item);
        }
    }
 
    void Pickup(Item item)
    {
        heldItem = item;
        item.OnPickUp(holdPoint);
    }
 
    public void DropItem()
    {
        if (heldItem == null) return;
 
        heldItem.OnDrop();
        heldItem = null;
    }
}