using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTriggerTimeline : MonoBehaviour
{
    [Header("Cutscene")]
    public PlayableDirector cutsceneDirector; // Timeline da cutscene
    public GameObject fadePanel;              // Painel de fade
    public float fadeDuration = 1.5f;         // dura��o do fade

    private CanvasGroup fadeGroup;
    private bool triggered = false;

    [Header("Player")]
    public GameObject playerCamera; // arraste a câmera do jogador aqui
    public GameObject playerLantern; // opcional: arraste o objeto da lanterna se quiser desligar

    private void Start()
    {
        // Prepara painel
        fadeGroup = fadePanel.GetComponent<CanvasGroup>();
        fadeGroup.alpha = 0f;
        fadePanel.SetActive(false);

        // Bloqueia cursor no in�cio
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Garante que a Timeline n�o toque automaticamente
        cutsceneDirector.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            // Desliga a câmera do jogador
            if (playerCamera != null)
                playerCamera.SetActive(false);

            // Desliga a lanterna (opcional)
            if (playerLantern != null)
                playerLantern.SetActive(false);

            // Ativa a câmera da cutscene
            cutsceneDirector.gameObject.SetActive(true);

            cutsceneDirector.Play();
            cutsceneDirector.stopped += OnCutsceneFinished;
        }
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        // Remove callback para evitar erros
        director.stopped -= OnCutsceneFinished;

        // Inicia fade do painel
        StartCoroutine(FadeInPanel());

        // C�mera permanece na posi��o final da cutscene
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

        fadeGroup.alpha = 1f; // Painel totalmente vis�vel

        // Desbloqueia cursor apenas ap�s o fade-in
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}