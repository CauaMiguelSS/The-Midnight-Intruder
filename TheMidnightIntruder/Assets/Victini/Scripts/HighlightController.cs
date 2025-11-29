using UnityEngine;

// Anexar este script a todos os itens interativos (ex: KeyItem)
public class HighlightController : MonoBehaviour
{
    // A cor da borda que deve aparecer
    public Color highlightColor = Color.white;
    
    // A cor original (guardamos para voltar ao normal)
    private Color originalColor;
    private Renderer itemRenderer;
    private Material itemMaterial;

    void Start()
    {
        itemRenderer = GetComponent<Renderer>();
        if (itemRenderer == null)
        {
            Debug.LogError("Renderer não encontrado no item " + gameObject.name);
            enabled = false;
            return;
        }

        // Obtém a referência ao material
        itemMaterial = itemRenderer.material;
        
        // Verifica se o material tem uma propriedade _Color para guardar a cor original
        if (itemMaterial.HasProperty("_Color"))
        {
            originalColor = itemMaterial.GetColor("_Color");
        }
        else
        {
            // Se o shader não tem _Color, a borda deve ser feita de outra forma (outline shader)
            // Para simplificar, assumiremos que o material tem _Color
            Debug.LogWarning("O material não tem a propriedade _Color. A borda pode não funcionar.");
        }
    }

    // Chamado pelo PlayerHighlight.cs quando o mouse passa por cima
    public void Highlight(bool state)
    {
        if (itemMaterial.HasProperty("_Color"))
        {
            if (state)
            {
                // Ativa o destaque (muda para a cor branca)
                itemMaterial.SetColor("_Color", highlightColor);
            }
            else
            {
                // Desativa o destaque (volta para a cor original)
                itemMaterial.SetColor("_Color", originalColor);
            }
        }

        // Se você estiver usando um componente de Outline Shader, ativaria ele aqui.
        // Ex: itemOutlineComponent.enabled = state; 
    }
}