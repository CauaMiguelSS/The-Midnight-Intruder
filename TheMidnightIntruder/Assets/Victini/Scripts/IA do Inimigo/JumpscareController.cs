using UnityEngine;

public class JumpscareController : MonoBehaviour
{
    [Header("Player")]
    public GameObject playerCamera;                  // Câmera do jogador
    public Rigidbody playerRigidbody;                // Rigidbody do jogador
    public MonoBehaviour[] playerMovementScripts;    // Scripts de movimento do jogador

    [Header("Enemies")]
    public GameObject normalEnemy;                   // Inimigo normal
    public MonoBehaviour enemyAIScript;              // Script de IA do inimigo normal
    public GameObject jumpscareEnemy;                // Inimigo do jumpscare
    public Transform lookAtPoint;                    // Ponto que o jogador deve olhar
    public float minDistanceFromPlayer = 2f;         // Distância mínima que o inimigo deve manter

    [Header("UI & Audio")]
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

        // 🔹 Trava jogador
        PlayerLock.IsLocked = true;

        // 🔹 Congela Rigidbody do jogador
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            playerRigidbody.isKinematic = true;
        }

        // 🔹 Desativa scripts de movimento do jogador
        foreach (var script in playerMovementScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        // 🔹 Faz o jogador olhar para o ponto fixo do inimigo
        if (lookAtPoint != null)
        {
            Vector3 direction = (lookAtPoint.position - playerCamera.transform.position).normalized;
            playerCamera.transform.rotation = Quaternion.LookRotation(direction);
        }

        // 🔹 Congela inimigo normal
        if (normalEnemy != null)
        {
            normalEnemy.SendMessage("FreezeEnemy", SendMessageOptions.DontRequireReceiver);

            // Faz o inimigo olhar para o jogador
            normalEnemy.transform.LookAt(playerCamera.transform.position);

            // 🔹 Desativa script de IA do inimigo
            if (enemyAIScript != null)
                enemyAIScript.enabled = false;

            // 🔹 Mantém distância mínima do jogador
            MaintainDistance(normalEnemy.transform, playerCamera.transform, minDistanceFromPlayer);
        }

        // 🔹 Ativa o inimigo do jumpscare
        if (jumpscareEnemy != null)
            jumpscareEnemy.SetActive(true);

        // 🔹 Toca o som do jumpscare
        if (jumpscareSound != null)
            jumpscareSound.Play();

        // 🔹 Mostra tela de morte após delay
        Invoke(nameof(ShowDeathScreen), 0.8f);
    }

    // Mantém o inimigo a uma distância mínima do jogador
    private void MaintainDistance(Transform enemy, Transform player, float minDistance)
    {
        Vector3 offset = enemy.position - player.position;
        float currentDistance = offset.magnitude;

        if (currentDistance < minDistance)
        {
            Vector3 direction = offset.normalized;
            enemy.position = player.position + direction * minDistance;
        }
    }

    void ShowDeathScreen()
    {
        deathScreen.SetActive(true);

        // Libera cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}