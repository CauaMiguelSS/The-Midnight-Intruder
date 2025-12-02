using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroSequenceManager_Final : MonoBehaviour
{
    // --- Configurações no Inspector ---

    [Header("UI Elements")]
    public CanvasGroup fadePanelCanvasGroup; 
    public TMP_Text initialFadeInText;      
    public TMP_Text typingEffectText;       

    [Header("Audio Settings")]
    public AudioSource gameAudioSource;
    public AudioClip sequenceSound;          // O som principal da sequência
    
    [Header("Text Settings")]
    [TextArea(3, 10)]
    public string initialTextContent = "Bem-vindo à escuridão...";
    [TextArea(3, 10)]
    public string typingTextContent = "Você nunca deveria ter vindo aqui. Agora é tarde.";
    public float typingSpeed = 0.05f;

    [Header("Timing Settings")]
    public float shortFadeDuration = 1.5f;
    public float longFadeDuration = 2.5f;
    public float waitBeforeTyping = 1f;       // Tempo de espera antes de começar a digitação

    [Header("Player Control")]
    public MonoBehaviour cameraMovementScript;

    // --- Controle de Estado ---
    private bool sequenceStarted = false;

    void Start()
    {
        // Configuração inicial da UI (o mesmo que antes)
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

    // --- Corrotina Principal de Sequência ---

    IEnumerator RunIntroSequence()
    {
        // 1. TRAVAR CÂMERA
        ToggleCameraControl(false);
        
        // 2. TEXTO 1: FADE-IN
        // O texto faz o fade-in e, por não ter um fade-out separado, ficará visível 
        // até o painel (que o contém) desaparecer.
        initialFadeInText.text = initialTextContent;
        yield return StartCoroutine(FadeTMPText(initialFadeInText, 0f, 1f, shortFadeDuration));

        // 3. ÁUDIO: COMEÇA A TOCAR 
        if (gameAudioSource != null && sequenceSound != null)
        {
            gameAudioSource.clip = sequenceSound;
            gameAudioSource.Play();
        }
        
        // 4. ESPERA ANTES DE DIGITAR
        yield return new WaitForSeconds(waitBeforeTyping); 

        // 5. TEXTO 2: EFEITO DE DIGITAÇÃO
        typingEffectText.alpha = 1f;
        // Corrotina de digitação simplificada (sem áudio de tecla)
        yield return StartCoroutine(TypeWriterEffect(
            typingTextContent, 
            typingEffectText, 
            typingSpeed
        ));

        // 6. ESPERA APÓS A DIGITAÇÃO
        yield return new WaitForSeconds(2f);

        // 7. PAINEL: FADE-OUT FINAL
        // O painel preto desaparecerá, levando consigo os textos.
        yield return StartCoroutine(FadeCanvasGroup(fadePanelCanvasGroup, 1f, 0f, longFadeDuration));
        
        // 8. DESTROVA CÂMERA
        ToggleCameraControl(true);
        fadePanelCanvasGroup.blocksRaycasts = false; 
    }

    // --- Funções Auxiliares de Corrotina ---

    // ATUALIZADO: Corrotina de Digitação SIMPLIFICADA (sem parâmetros de áudio)
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

    // Corrotina para fazer o Fade de um TMP_Text (inalterada)
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

    // Função para Fade de Canvas Group (inalterada)
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

    // Sua Função de Controle de Câmera (inalterada)
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