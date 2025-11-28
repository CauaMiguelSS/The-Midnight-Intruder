using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpscareController : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject normalEnemy;
    public GameObject jumpscareEnemy;
    public GameObject deathScreen;
    public AudioSource jumpscareSound;

    private bool triggered = false;

    void Start()
    {
        deathScreen.SetActive(false);
        if (jumpscareEnemy != null)
            jumpscareEnemy.SetActive(false);
    }

    public void TriggerJumpscare()
    {
        if (triggered) return;
        triggered = true;

        PlayerLock.IsLocked = true;

        if (normalEnemy != null)
        {
            normalEnemy.SendMessage("FreezeEnemy", SendMessageOptions.DontRequireReceiver);
        }

        var controller = playerCamera.GetComponentInParent<CharacterController>();
        if (controller != null)
            controller.enabled = false;

        jumpscareEnemy.SetActive(true);

        if (jumpscareSound != null)
            jumpscareSound.Play();

        Invoke(nameof(ShowDeathScreen), 0.8f);
    }

    void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        PlayerLock.IsLocked = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}