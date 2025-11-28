using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JumpscareController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject jumpscareObject;   // O modelo/monstro que aparece na cara do player
    public AudioSource jumpscareSound;
    public Image fadeImage;              // UI preta na tela

    [Header("Settings")]
    public float fadeSpeed = 2f;
    public float jumpscareDelay = 1.3f;  // Tempo até reiniciar/resetar

    private bool isActive = false;
    private float fadeAmount = 0f;

    void Start()
    {
        jumpscareObject.SetActive(false);
        fadeImage.color = new Color(0, 0, 0, 0); // transparente
    }

    void Update()
    {
        if (isActive)
        {
            fadeAmount += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, fadeAmount);
        }
    }

    public void TriggerJumpscare()
    {
        if (isActive) return;

        isActive = true;

        // travar movimento
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // desabilitar controles do jogador
        var ctrl = playerCamera.GetComponentInParent<CharacterController>();
        if (ctrl) ctrl.enabled = false;

        jumpscareObject.SetActive(true);
        jumpscareSound.Play();

        // olha o jumpscare diretamente
        playerCamera.transform.LookAt(jumpscareObject.transform);

        Invoke("RestartScene", jumpscareDelay);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
