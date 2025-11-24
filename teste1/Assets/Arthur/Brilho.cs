using UnityEngine;
using UnityEngine.UI;

public class BrightnessControl : MonoBehaviour
{
    public Image overlay; // arraste a imagem preta aqui no Inspector
    [Range(0, 1)] public float brightness = 1f; // 1 = claro, 0 = escuro

    void Update()
    {
        Color c = overlay.color;
        c.a = 1f - brightness;
        overlay.color = c;
    }
}
