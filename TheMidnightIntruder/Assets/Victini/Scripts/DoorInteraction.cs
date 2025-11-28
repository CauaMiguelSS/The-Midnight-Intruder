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
        
            // 1. Verifica se a porta e o jogador existem.
            if (door != null && player != null)
            {
                // 2. Acessa o item que o jogador está segurando (heldItem é agora público/acessível).
                // (Use player.GetHeldItem se você usou a Opção 2)
                Item itemHeld = player.GetHeldItem; 
            
                // 3. Tenta converter o Item genérico para KeyItem (cast).
                // Se o item não for uma chave, 'key' será null.
                KeyItem key = itemHeld as KeyItem;

                // 4. Se for uma KeyItem válida, tenta destravar a porta.
                if (key != null)
                {
                door.TryUnlock(key);
                }
            }
        }
    }
}