using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    public float pickupDistance = 3f;
    public Transform holdPoint;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;
    public Camera playerCamera; // <<< ADICIONADO

    private Vector3 originalScale;
    private Transform originalParent;

    private Item heldItem;

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

        // <<< USA A CÂMERA DO JOGADOR
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupDistance))
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

    void DropItem()
    {
        if (heldItem == null) return;

        heldItem.OnDrop();
        heldItem = null;
    }

    public void OnPickUp(Transform holdPoint)
    {
        originalScale = transform.localScale;
        originalParent = transform.parent;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        transform.localScale = originalScale;
    }

    public void OnDrop()
    {
        transform.SetParent(null);

        transform.localScale = originalScale;
    }
}