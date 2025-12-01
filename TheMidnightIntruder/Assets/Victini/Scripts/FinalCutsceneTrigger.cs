using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using System.Collections;

public class FinalCutsceneTrigger : MonoBehaviour
{
    [Header("Cutscene")]
    public PlayableDirector cutscene;
    public Camera playerCamera;
    public Camera cutsceneCamera;

    [Header("Lanterna")]
    public GameObject flashlightObject; // <= ADICIONADO

    [Header("UI Final")]
    public CanvasGroup fadeGroup;       // fundo preto
    public CanvasGroup uiGroup;         // texto + botão
    public float fadeDuration = 2f;

    [Header("Audio")]
    public AudioSource breathingAudio;
    public float audioFadeOutDuration = 2f;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;
        if (!other.CompareTag("Player")) return;

        hasPlayed = true;
        StartCutscene();
    }

    void StartCutscene()
    {
        // desativar lanterna ao iniciar cutscene
        if (flashlightObject != null)
            flashlightObject.SetActive(false);

        // troca de câmera
        if (playerCamera != null) playerCamera.enabled = false;
        if (cutsceneCamera != null) cutsceneCamera.enabled = true;

        // iniciar som ofegante
        if (breathingAudio != null)
            breathingAudio.Play();

        // inicia timeline
        cutscene.stopped += OnCutsceneFinished;
        cutscene.Play();
    }

    void OnCutsceneFinished(PlayableDirector pd)
    {
        // fade UI
        StartCoroutine(FadeIn());

        // fade-out do áudio
        if (breathingAudio != null)
            StartCoroutine(FadeOutAudio());

        // liberar cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator FadeIn()
    {
        fadeGroup.gameObject.SetActive(true);
        uiGroup.gameObject.SetActive(true);

        fadeGroup.alpha = 0;
        uiGroup.alpha = 0;

        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = t / fadeDuration;

            fadeGroup.alpha = a;
            uiGroup.alpha = a;

            yield return null;
        }

        fadeGroup.alpha = 1;
        uiGroup.alpha = 1;
    }

    IEnumerator FadeOutAudio()
    {
        float startVolume = breathingAudio.volume;
        float t = 0f;

        while (t < audioFadeOutDuration)
        {
            t += Time.deltaTime;
            breathingAudio.volume = Mathf.Lerp(startVolume, 0f, t / audioFadeOutDuration);
            yield return null;
        }

        breathingAudio.volume = 0f;
        breathingAudio.Stop();
        breathingAudio.volume = startVolume; // opcional
    }
}