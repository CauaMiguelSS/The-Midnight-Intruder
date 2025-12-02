using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroSequenceManager_Final : MonoBehaviour
{

    [Header("UI Elements")]
    public CanvasGroup fadePanelCanvasGroup; 
    public TMP_Text initialFadeInText;      
    public TMP_Text typingEffectText;       

    [Header("Audio Settings")]
    public AudioSource gameAudioSource;
    public AudioClip sequenceSound;
    
    [Header("Text Settings")]
    [TextArea(3, 10)]
    public string initialTextContent = "Bem-vindo à escuridão...";
    [TextArea(3, 10)]
    public string typingTextContent = "Você nunca deveria ter vindo aqui. Agora é tarde.";
    public float typingSpeed = 0.05f;

    [Header("Timing Settings")]
    public float shortFadeDuration = 1.5f;
    public float longFadeDuration = 2.5f;
    public float waitBeforeTyping = 1f;

    [Header("Player Control")]
    public MonoBehaviour cameraMovementScript;

    private bool sequenceStarted = false;

    void Start()
    {
        if (fadePanelCanvasGroup != null)
        {
            fadePanelCanvasGroup.alpha = 1f;
            fadePanelCanvasGroup.blocksRaycasts = true;
        }
        initialFadeInText.alpha = 0f;
        typingEffectText.alpha = 0f;

        if (!sequenceStarted)
        {
            StartCoroutine(RunIntroSequence());
            sequenceStarted = true;
        }
    }

    IEnumerator RunIntroSequence()
    {
        ToggleCameraControl(false);

        initialFadeInText.text = initialTextContent;
        yield return StartCoroutine(FadeTMPText(initialFadeInText, 0f, 1f, shortFadeDuration));

        if (gameAudioSource != null && sequenceSound != null)
        {
            gameAudioSource.clip = sequenceSound;
            gameAudioSource.Play();
        }
        
        yield return new WaitForSeconds(waitBeforeTyping); 

        typingEffectText.alpha = 1f;
        yield return StartCoroutine(TypeWriterEffect(
            typingTextContent, 
            typingEffectText, 
            typingSpeed
        ));

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(FadeCanvasGroup(fadePanelCanvasGroup, 1f, 0f, longFadeDuration));
        
        ToggleCameraControl(true);
        fadePanelCanvasGroup.blocksRaycasts = false; 
    }

    IEnumerator TypeWriterEffect(
        string fullText, 
        TMP_Text targetText, 
        float delay)
    {
        targetText.text = ""; 
        for (int i = 0; i < fullText.Length; i++)
        {
            targetText.text += fullText[i];
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator FadeTMPText(TMP_Text textComponent, float startAlpha, float endAlpha, float duration)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;
        Color color = textComponent.color;

        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            textComponent.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }
        textComponent.color = new Color(color.r, color.g, color.b, endAlpha);
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float startAlpha, float endAlpha, float duration)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null; 
        }
        cg.alpha = endAlpha;
    }

    private void ToggleCameraControl(bool isEnabled)
    {
        if (cameraMovementScript != null)
        {
            cameraMovementScript.enabled = isEnabled;
        }
        else
        {
            Debug.LogError("O campo 'Camera Movement Script' não foi preenchido no Inspector!");
        }
    }
}