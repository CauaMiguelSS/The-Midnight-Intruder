using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTriggerTimeline : MonoBehaviour
{
    [Header("Cutscene")]
    public PlayableDirector cutsceneDirector;
    public GameObject fadePanel;
    public float fadeDuration = 1.5f;

    private CanvasGroup fadeGroup;
    private bool triggered = false;

    [Header("Player")]
    public GameObject playerCamera;
    public GameObject playerLantern;

    private void Start()
    {
        fadeGroup = fadePanel.GetComponent<CanvasGroup>();
        fadeGroup.alpha = 0f;
        fadePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cutsceneDirector.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (playerCamera != null)
                playerCamera.SetActive(false);

            if (playerLantern != null)
                playerLantern.SetActive(false);

            cutsceneDirector.gameObject.SetActive(true);

            cutsceneDirector.Play();
            cutsceneDirector.stopped += OnCutsceneFinished;
        }
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        director.stopped -= OnCutsceneFinished;

        StartCoroutine(FadeInPanel());

    }

    private System.Collections.IEnumerator FadeInPanel()
    {
        fadePanel.SetActive(true);
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        fadeGroup.alpha = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}