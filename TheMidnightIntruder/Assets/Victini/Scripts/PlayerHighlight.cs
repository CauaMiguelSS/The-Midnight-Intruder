using UnityEngine;
using UnityEngine.UI; // Necessário para Text/Text Mesh Pro

public class PlayerHighlight : MonoBehaviour
{
    // Distância de detecção do mouse
    public float highlightDistance = 3f;
    
    // UI: Onde o texto de interação ("Aperte E para pegar") será exibido
    public Text interactionText; // Se estiver usando Text Mesh Pro, use 'TextMeshProUGUI'
    public Camera playerCamera; 

    private HighlightController currentHighlight; // Item atualmente destacado

    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
        
        // Inicialmente esconde o texto
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    void Update()
    {
        TryHighlightItem();
    }

    void TryHighlightItem()
    {
        RaycastHit hit;

        if (playerCamera == null) return;

        // O Raycast deve ser o mesmo que o de pegar itens
        if (Physics.Raycast(playerCamera.transform.position, 
                            playerCamera.transform.forward, 
                            out hit, 
                            highlightDistance))
        {
            // Tenta encontrar o script HighlightController no objeto atingido
            HighlightController hitHighlight = hit.collider.GetComponent<HighlightController>();

            if (hitHighlight != null)
            {
                // 1. Encontrou um novo item ou continua no mesmo item
                if (currentHighlight != hitHighlight)
                {
                    // Desativa o destaque do item anterior, se houver
                    if (currentHighlight != null)
                        currentHighlight.Highlight(false);

                    // Ativa o destaque no novo item
                    currentHighlight = hitHighlight;
                    currentHighlight.Highlight(true);
                    
                    // Mostra o texto
                    ShowText("Aperte 'E' para interagir"); 
                }
            }
            else
            {
                // 2. Acertou algo, mas não é um item interativo
                DisableHighlight();
            }
        }
        else
        {
            // 3. Não acertou nada
            DisableHighlight();
        }
    }

    void DisableHighlight()
    {
        // Se estava destacando um item, desativa o destaque
        if (currentHighlight != null)
        {
            currentHighlight.Highlight(false);
            currentHighlight = null;
        }

        // Esconde o texto
        ShowText(""); 
    }

    void ShowText(string message)
    {
        if (interactionText != null)
        {
            if (string.IsNullOrEmpty(message))
            {
                // Se a mensagem está vazia, esconde o texto
                interactionText.gameObject.SetActive(false);
            }
            else
            {
                // Se houver mensagem, mostra o texto e atualiza a mensagem
                interactionText.text = message;
                interactionText.gameObject.SetActive(true);
            }
        }
    }
}