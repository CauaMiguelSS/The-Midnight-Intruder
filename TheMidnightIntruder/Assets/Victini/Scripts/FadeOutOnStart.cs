using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 
using System.Collections; 
using TMPro; // Use este se estiver usando TextMeshPro

public class FadeOutOnStart : MonoBehaviour
{
    [Header("Elementos de Fade")]
    // Arraste o seu Texto e a sua Imagem de Logo/Splash para estes campos
    [SerializeField] private Graphic logoImage; 
    [SerializeField] private Graphic introText; 

    [Header("Configurações de Tempo")]
    [Tooltip("Duração do Fade (In e Out) em segundos")]
    [SerializeField] private float fadeDuration = 1.5f;

    [Tooltip("Tempo que os elementos ficam totalmente visíveis após o Fade-In e antes do Fade-Out")]
    [Range(3f, 7f)]
    [SerializeField] private float visibleDuration = 5f; 

    [Header("Próxima Cena")]
    [SerializeField] private string nextSceneName = "GameLevel1"; 

    void Start()
    {
        // ⚠️ IMPORTANTE: Garantir que os elementos comecem invisíveis para o Fade-In
        SetInitialAlpha(logoImage, 0f);
        SetInitialAlpha(introText, 0f);

        // Inicia a Coroutine que executa toda a sequência
        StartCoroutine(ExecuteSequence());
    }

    // --- FUNÇÕES DE AUXÍLIO ---

    private void SetInitialAlpha(Graphic graphic, float alpha)
    {
        if (graphic != null)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }

    // --- COROUTINE PRINCIPAL ---

    private IEnumerator ExecuteSequence()
    {
        // 1. FADE IN (Começam invisíveis e ficam visíveis)
        Debug.Log("Iniciando Fade-In...");
        StartCoroutine(FadeGraphic(logoImage, 0f, 1.5f, fadeDuration));
        StartCoroutine(FadeGraphic(introText, 0f, 1.5f, fadeDuration));
        
        // Espera o Fade-In terminar
        yield return new WaitForSeconds(fadeDuration);

        // 2. TEMPO VISÍVEL (Ficam parados na tela)
        Debug.Log("Elementos visíveis por " + visibleDuration + " segundos.");
        yield return new WaitForSeconds(visibleDuration);

        // 3. FADE OUT (Ficam invisíveis)
        Debug.Log("Iniciando Fade-Out...");
        StartCoroutine(FadeGraphic(logoImage, 1f, 0f, fadeDuration));
        StartCoroutine(FadeGraphic(introText, 1f, 0f, fadeDuration));

        // Espera o Fade-Out terminar
        yield return new WaitForSeconds(fadeDuration);

        // 4. CARREGA A PRÓXIMA CENA
        Debug.Log("Carregando a cena: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }
    
    // --- FUNÇÃO AUXILIAR PARA O FADE GENÉRICO ---
    
    private IEnumerator FadeGraphic(Graphic graphic, float startAlpha, float endAlpha, float duration)
    {
        if (graphic == null) yield break; 

        float startTime = Time.time;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            
            // Interpola a opacidade entre o valor inicial e final
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            
            Color color = graphic.color;
            color.a = newAlpha;
            graphic.color = color;

            yield return null;
        }
        
        // Garante que o elemento atinja a opacidade final
        Color finalColor = graphic.color;
        finalColor.a = endAlpha;
        graphic.color = finalColor;
    }
}