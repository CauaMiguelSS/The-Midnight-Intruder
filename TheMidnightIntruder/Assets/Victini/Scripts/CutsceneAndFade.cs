using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTriggerTimeline : MonoBehaviour
{
    [Header("Cutscene")]
    public PlayableDirector cutsceneDirector; // Timeline da cutscene
    public GameObject fadePanel;              // Painel de fade
    public float fadeDuration = 1.5f;         // duração do fade

    private CanvasGroup fadeGroup;
    private bool triggered = false;

    private void Start()
    {
        // Prepara painel
        fadeGroup = fadePanel.GetComponent<CanvasGroup>();
        fadeGroup.alpha = 0f;
        fadePanel.SetActive(false);

        // Bloqueia cursor no início
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Garante que a Timeline não toque automaticamente
        cutsceneDirector.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            // Ativa a CutsceneCamera
            cutsceneDirector.gameObject.SetActive(true);

            // Toca a Timeline
            cutsceneDirector.Play();

            // Assina evento para saber quando a Timeline terminou
            cutsceneDirector.stopped += OnCutsceneFinished;
        }
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        // Remove callback para evitar erros
        director.stopped -= OnCutsceneFinished;

        // Inicia fade do painel
        StartCoroutine(FadeInPanel());

        // Câmera permanece na posição final da cutscene
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

        fadeGroup.alpha = 1f; // Painel totalmente visível

        // Desbloqueia cursor apenas após o fade-in
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}