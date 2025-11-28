using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class JumpscareController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject jumpscareObject;   // modelo/monstro que aparece
    public AudioSource jumpscareSound;
    public Image fadeImage;              // UI preta (Image full-screen)

    [Header("Settings")]
    public float fadeSpeed = 2f;
    public float jumpscareDelay = 1.3f;  // tempo até reiniciar/resetar
    public float lookAtSpeed = 10f;

    private bool isActive = false;
    private float fadeAmount = 0f;

    void Start()
    {
        // Proteções iniciais
        if (jumpscareObject != null) jumpscareObject.SetActive(false);
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);                    // mantenha ativa para o Canvas funcionar
            fadeImage.color = new Color(0, 0, 0, 0);                // alpha 0 no início
        }

        if (jumpscareSound != null)
        {
            // garante que não toque ao iniciar
            jumpscareSound.playOnAwake = false;
            jumpscareSound.loop = false;
            jumpscareSound.Stop();
        }
    }

    void Update()
    {
        if (!isActive) return;

        // fade para preto
        fadeAmount = Mathf.Clamp01(fadeAmount + Time.deltaTime * fadeSpeed);
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, fadeAmount);

        // opcional: força a câmera olhar pro jumpscare suavemente
        if (playerCamera != null && jumpscareObject != null)
        {
            Vector3 dir = (jumpscareObject.transform.position - playerCamera.transform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir);
            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, targetRot, Time.deltaTime * lookAtSpeed);
        }
    }

    public void TriggerJumpscare()
    {
        if (isActive) return;
        isActive = true;

        // garante que a UI está visível (alpha 0) para iniciar o fade sem "pulo"
        if (fadeImage != null) fadeImage.gameObject.SetActive(true);

        // travar cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // desabilitar scripts do jogador de forma genérica
        if (playerCamera != null)
        {
            var playerRoot = playerCamera.transform.root;
            // desliga todos os MonoBehaviours no root (exceto este script se por acaso estiver no mesmo root)
            MonoBehaviour[] scripts = playerRoot.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var s in scripts)
            {
                if (s == null) continue;
                // NÃO desabilite este JumpscareController caso esteja no mesmo objeto
                if (s == this) continue;
                // Não desative UI importantes: (opcional) você pode filtrar por nome de script aqui
                try { s.enabled = false; } catch { }
            }
        }

        // ativa visuals e som
        if (jumpscareObject != null) jumpscareObject.SetActive(true);
        if (jumpscareSound != null) jumpscareSound.Play();

        // reinicia cena depois de um tempo
        StartCoroutine(RestartAfterDelay(jumpscareDelay));
    }

    IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}