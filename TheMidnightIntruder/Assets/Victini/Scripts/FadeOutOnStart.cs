using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 
using System.Collections;

public class FadeOutOnStart : MonoBehaviour
{
    [Header("Elementos de Fade")]
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
        SetInitialAlpha(logoImage, 0f);
        SetInitialAlpha(introText, 0f);

        StartCoroutine(ExecuteSequence());
    }

    private void SetInitialAlpha(Graphic graphic, float alpha)
    {
        if (graphic != null)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }

    private IEnumerator ExecuteSequence()
    {
        Debug.Log("Iniciando Fade-In...");
        StartCoroutine(FadeGraphic(logoImage, 0f, 1.5f, fadeDuration));
        StartCoroutine(FadeGraphic(introText, 0f, 1.5f, fadeDuration));
        
        yield return new WaitForSeconds(fadeDuration);

        Debug.Log("Elementos visíveis por " + visibleDuration + " segundos.");
        yield return new WaitForSeconds(visibleDuration);

        Debug.Log("Iniciando Fade-Out...");
        StartCoroutine(FadeGraphic(logoImage, 1f, 0f, fadeDuration));
        StartCoroutine(FadeGraphic(introText, 1f, 0f, fadeDuration));

        yield return new WaitForSeconds(fadeDuration);

        Debug.Log("Carregando a cena: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }
    
    private IEnumerator FadeGraphic(Graphic graphic, float startAlpha, float endAlpha, float duration)
    {
        if (graphic == null) yield break; 

        float startTime = Time.time;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            
            Color color = graphic.color;
            color.a = newAlpha;
            graphic.color = color;

            yield return null;
        }

        Color finalColor = graphic.color;
        finalColor.a = endAlpha;
        graphic.color = finalColor;
    }
}