using UnityEngine;

public class FadePanel : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1.5f;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false); // painel começa desativado
    }

    public void StartFadeIn()
    {
        gameObject.SetActive(true); // ativa antes de iniciar a corrotina
        StartCoroutine(FadeInRoutine());
    }

    private System.Collections.IEnumerator FadeInRoutine()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f; // totalmente visível
    }
}
