using UnityEngine;
using TMPro;
using System.Collections;

public class StartFadeWithText : MonoBehaviour
{
    [Header("Fade")]
    public CanvasGroup fadeGroup;
    public float fadeDuration = 2f;

    [Header("Typewriter")]
    public TextMeshProUGUI textUI;
    [TextArea] public string fullText = "O que está acontecendo?";
    public float typeDelay = 0.05f;
    public float delayBeforeFade = 1.5f;

    [Header("Camera Control")]
    [Tooltip("Arraste o script que controla o movimento/rotação da câmera/jogador.")]
    public MonoBehaviour cameraMovementScript;

    private void Start()
    {
        fadeGroup.alpha = 1;
        textUI.text = "";

        ToggleCameraControl(false); 

        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        yield return StartCoroutine(TypewriterEffect());

        yield return new WaitForSeconds(delayBeforeFade);

        yield return StartCoroutine(FadeOut());

        ToggleCameraControl(true);

        gameObject.SetActive(false); 
    }

    IEnumerator TypewriterEffect()
    {
        textUI.text = "";

        foreach (char c in fullText)
        {
            textUI.text += c;
            yield return new WaitForSeconds(typeDelay);
        }
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeGroup.alpha = 1f - (timer / fadeDuration);
            yield return null;
        }

        fadeGroup.alpha = 0f;
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