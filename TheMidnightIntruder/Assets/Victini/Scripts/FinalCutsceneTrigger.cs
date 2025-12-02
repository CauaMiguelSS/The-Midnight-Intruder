using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class FinalCutsceneTrigger : MonoBehaviour
{
    [Header("Cutscene")]
    public PlayableDirector cutscene;
    public Camera playerCamera;
    public Camera cutsceneCamera;

    [Header("Lanterna")]
    public GameObject flashlightObject;

    [Header("UI Final")]
    public CanvasGroup fadeGroup;     // fundo preto
    public CanvasGroup finishPanel;   // texto + botões
    public float fadeDuration = 2f;

    [Header("Audio")]
    public AudioSource breathingAudio;

    private bool hasPlayed = false;

    private void Awake()
    {
        fadeGroup.alpha = 0f;
        finishPanel.alpha = 0f;

        fadeGroup.gameObject.SetActive(false);
        finishPanel.gameObject.SetActive(false);

        if (cutsceneCamera != null)
            cutsceneCamera.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;
        if (!other.CompareTag("Player")) return;

        hasPlayed = true;
        StartCutscene();
    }

    private void StartCutscene()
    {
        if (flashlightObject != null)
            flashlightObject.SetActive(false);

        if (playerCamera != null)
            playerCamera.enabled = false;

        cutsceneCamera.gameObject.SetActive(true);
        cutsceneCamera.enabled = true;

        if (breathingAudio != null)
            breathingAudio.Play();

        cutscene.stopped += OnCutsceneFinished;

        cutscene.time = 0;
        cutscene.Evaluate();
        cutscene.Play();
    }

    private void OnCutsceneFinished(PlayableDirector pd)
    {
        StartCoroutine(FadeBlackThenShowPanel());

        cutsceneCamera.enabled = false;
        playerCamera.enabled = true;

        cutscene.stopped -= OnCutsceneFinished;
        cutscene.Stop();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator FadeBlackThenShowPanel()
    {
        // FADE SOMENTE DO FUNDO PRETO
        fadeGroup.gameObject.SetActive(true);
        fadeGroup.alpha = 0f;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = t / fadeDuration;
            yield return null;
        }

        fadeGroup.alpha = 1f;

        // Agora ativa o painel
        finishPanel.gameObject.SetActive(true);
        finishPanel.alpha = 1f; // SEM FADE, para não sumir

    }
}

