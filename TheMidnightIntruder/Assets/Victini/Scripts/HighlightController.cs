using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HighlightController : MonoBehaviour
{
    [Header("Highlight")]
    public Color highlightColor = Color.yellow;
    [Range(0f, 2f)] public float pulseIntensity = 0.6f;   // quanto brilha no pulso
    [Range(0f, 5f)] public float pulseSpeed = 3f;         // velocidade do pulso
    public bool usePulse = true;

    [Header("Properties (automático detecta)")]
    public string[] colorPropertyNames = new string[] { "_Color", "_BaseColor", "_TintColor" };
    public string emissionPropertyName = "_EmissionColor";

    // internos
    private Renderer[] renderers;
    private MaterialPropertyBlock propBlock;
    private bool isHighlighted = false;
    private Coroutine pulseCoroutine;

    // guarda a cor original para fallback (não é necessário restaurar material por usar MPB,
    // mas guardamos para efeitos que precisem)
    private Dictionary<int, Color> originalColors = new Dictionary<int, Color>();

    void Awake()
    {
        // pega todos os renderers (inclui children)
        renderers = GetComponentsInChildren<Renderer>(includeInactive: false);
        if (renderers == null || renderers.Length == 0)
            Debug.LogWarning($"[Highlight] Nenhum Renderer encontrado em '{gameObject.name}'.");

        propBlock = new MaterialPropertyBlock();

        // opcional: tenta armazenar as cores originais via MPB read (se possível)
        CacheOriginalColors();
    }

    void OnDisable()
    {
        // garante limpar quando desativar
        ForceClear();
    }

    // tenta ler cor original (apenas para referência; MPB não altera materiais)
    void CacheOriginalColors()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            var r = renderers[i];
            if (r == null) continue;

            // para cada material do renderer, tenta obter cor (se houver)
            for (int matIndex = 0; matIndex < r.sharedMaterials.Length; matIndex++)
            {
                int key = GetMaterialKey(r, matIndex);
                if (!originalColors.ContainsKey(key))
                {
                    Color c = Color.white;
                    var mat = r.sharedMaterials[matIndex];
                    if (mat != null)
                    {
                        foreach (var pname in colorPropertyNames)
                        {
                            if (mat.HasProperty(pname))
                            {
                                c = mat.GetColor(pname);
                                break;
                            }
                        }
                    }
                    originalColors[key] = c;
                }
            }
        }
    }

    int GetMaterialKey(Renderer r, int matIndex)
    {
        // chave única por renderer + index (hash)
        return r.GetInstanceID() ^ (matIndex << 16);
    }

    // Public API
    public void Highlight(bool state)
    {
        if (state == isHighlighted) return;

        isHighlighted = state;

        if (isHighlighted)
        {
            ApplyHighlightImmediate();
            if (usePulse)
            {
                if (pulseCoroutine != null) StopCoroutine(pulseCoroutine);
                pulseCoroutine = StartCoroutine(PulseRoutine());
            }
        }
        else
        {
            if (pulseCoroutine != null) { StopCoroutine(pulseCoroutine); pulseCoroutine = null; }
            ClearHighlight();
        }
    }

    // Força esconder sem dependências externas
    public void ForceClear()
    {
        if (pulseCoroutine != null) { StopCoroutine(pulseCoroutine); pulseCoroutine = null; }
        ClearHighlight();
        isHighlighted = false;
    }

    // Aplica highlight imediatamente (seta MPB)
    void ApplyHighlightImmediate()
    {
        if (renderers == null) return;

        for (int i = 0; i < renderers.Length; i++)
        {
            var r = renderers[i];
            if (r == null) continue;

            int matCount = Mathf.Max(1, r.sharedMaterials.Length);

            for (int matIndex = 0; matIndex < matCount; matIndex++)
            {
                propBlock.Clear();

                // tenta usar _Color / _BaseColor / _TintColor usando MPB
                bool applied = false;
                foreach (var pname in colorPropertyNames)
                {
                    // escreve a propriedade no MPB (mesmo que o material não a use)
                    propBlock.SetColor(pname, highlightColor);
                    applied = true;
                }

                // Também tenta setar emissão, se o shader suportar
                propBlock.SetColor(emissionPropertyName, highlightColor * Mathf.LinearToGammaSpace(0.5f));

                r.SetPropertyBlock(propBlock, matIndex);
            }
        }
    }

    // Limpa o MPB para voltar ao visual não highlight
    void ClearHighlight()
    {
        if (renderers == null) return;

        for (int i = 0; i < renderers.Length; i++)
        {
            var r = renderers[i];
            if (r == null) continue;

            // limpar propriedade para cada material index
            int matCount = Mathf.Max(1, r.sharedMaterials.Length);
            for (int matIndex = 0; matIndex < matCount; matIndex++)
            {
                propBlock.Clear();
                // Se quisermos restaurar a cor original via MPB, podemos, mas
                // é mais seguro limpar (o material volta ao sharedMaterial)
                r.SetPropertyBlock(propBlock, matIndex);
            }
        }
    }

    // pulso suave da intensidade de highlight (aplica por MPB)
    IEnumerator PulseRoutine()
    {
        float t = 0f;
        while (true)
        {
            t += Time.deltaTime * pulseSpeed;
            float pulse = (Mathf.Sin(t) * 0.5f + 0.5f) * pulseIntensity; // 0..pulseIntensity
            // mistura a cor base com o pulso (aumentando a intensidade)
            Color c = Color.Lerp(highlightColor * 0.6f, highlightColor, pulse);

            for (int i = 0; i < renderers.Length; i++)
            {
                var r = renderers[i];
                if (r == null) continue;

                int matCount = Mathf.Max(1, r.sharedMaterials.Length);
                for (int matIndex = 0; matIndex < matCount; matIndex++)
                {
                    propBlock.Clear();
                    foreach (var pname in colorPropertyNames)
                        propBlock.SetColor(pname, c);

                    propBlock.SetColor(emissionPropertyName, c * Mathf.LinearToGammaSpace(0.5f));
                    r.SetPropertyBlock(propBlock, matIndex);
                }
            }

            yield return null;
        }
    }
}