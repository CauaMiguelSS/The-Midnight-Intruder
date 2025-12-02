using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using System.Collections;

public class FinalCutsceneTrigger : MonoBehaviour
{
    [Header("Cutscene")]
    public PlayableDirector cutscene;
    public Camera playerCamera;       // câmera do jogador
    public Camera cutsceneCamera;     // câmera da cutscene (filha animada)

    [Header("Lanterna")]
    public GameObject flashlightObject;

    [Header("UI Final")]
    public CanvasGroup fadeGroup;     // fundo preto
    public CanvasGroup uiGroup;       // texto + botões
    public float fadeDuration = 2f;

    [Header("Audio")]
    public AudioSource breathingAudio;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;
        if (!other.CompareTag("Player")) return;

        hasPlayed = true;
        StartCutscene();
    }

    private void StartCutscene()
    {
        // Desativa lanterna
        if (flashlightObject != null)
            flashlightObject.SetActive(false);

        // Desativa câmera do jogador
        if (playerCamera != null)
            playerCamera.enabled = false;

        // Ativa câmera da cutscene
        if (cutsceneCamera != null)
        {
            cutsceneCamera.gameObject.SetActive(true);
            cutsceneCamera.enabled = true;
        }

        // Toca áudio de respiração
        if (breathingAudio != null)
            breathingAudio.Play();

        // Assina evento de fim da cutscene
        cutscene.stopped += OnCutsceneFinished;

        // Reinicia timeline do começo
        cutscene.time = 0;
        cutscene.Evaluate();
        cutscene.Play();
    }

    private void OnCutsceneFinished(PlayableDirector pd)
    {
        // Inicia fade-in do UI que ficará ativo permanentemente
        StartCoroutine(FadeInUI());

        // Reativa câmera do jogador e desativa a cutscene
        if (playerCamera != null)
            playerCamera.enabled = true;

        if (cutsceneCamera != null)
            cutsceneCamera.enabled = false;

        // Remove evento para evitar chamadas repetidas
        cutscene.stopped -= OnCutsceneFinished;
        cutscene.Stop();

        // Libera cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator FadeInUI()
    {
        // Ativa os objetos de UI na cena
        fadeGroup.gameObject.SetActive(true);
        uiGroup.gameObject.SetActive(true);

        // Começa do alpha 0
        fadeGroup.alpha = 0f;
        uiGroup.alpha = 0f;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);

            // Define alpha diretamente
            fadeGroup.alpha = alpha;
            uiGroup.alpha = alpha;

            // Força todos os filhos a manter alpha 1 no uiGroup
            foreach (CanvasGroup cg in uiGroup.GetComponentsInChildren<CanvasGroup>(true))
            {
                cg.alpha = 1f;
            }

            yield return null;
        }

        // Garante que o alpha final seja 1
        fadeGroup.alpha = 1f;
        uiGroup.alpha = 1f;

        // Mantém os objetos ativos na cena permanentemente
    }
}