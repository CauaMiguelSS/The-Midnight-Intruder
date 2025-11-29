using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public KeyCode interactKey = KeyCode.E;
    // Adicionado referência à Câmera para Raycast mais preciso
    public Camera playerCamera; 

    private PlayerItemPickup player;

    void Start()
    {
        player = GetComponent<PlayerItemPickup>();
        
        // Se a câmera não foi configurada, tenta pegar a principal
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
        
        // Usando a posição e direção da CÂMERA para maior precisão
        if (playerCamera == null) return;
        
        if (Physics.Raycast(playerCamera.transform.position, 
                            playerCamera.transform.forward, 
                            out hit, 
                            interactionDistance))
        {
            LockedDoor door = hit.collider.GetComponent<LockedDoor>();
            
            if (door != null && player != null)
            {
                // 🔑 CORREÇÃO CRÍTICA: Acessa o item segurado (HeldItem) 
                // e tenta convertê-lo para KeyItem.
                KeyItem key = player.HeldItem as KeyItem; 

                if (key != null)
                {
                    door.TryUnlock(key, player); // Passa o jogador para o LockedDoor
                }
            }
        }
    }
}