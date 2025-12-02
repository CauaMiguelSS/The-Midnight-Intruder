using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class JumpscareController : MonoBehaviour
{
    [Header("Player & Enemy")]
    public GameObject playerCamera;
    public GameObject normalEnemy;
    public GameObject jumpscareEnemy;
    public GameObject deathScreen;
    public AudioSource jumpscareSound;

    [Header("Camera Look Settings")]
    public Transform lookTarget;
    public float lookSpeed = 10f;

    [Header("Enemy Freeze Settings")]
    public float freezeDistanceFromPlayer = 1.5f; // distância do inimigo na frente do player

    private bool triggered = false;
    private bool forceLook = false;
    private bool lockPosition = false;
    private bool freezeEnemyTransform = false;

    private Vector3 frozenPosition;
    private Vector3 frozenEnemyPos;

    private Rigidbody rbPlayer;
    private Rigidbody rbJumpscareEnemy;

    void Start()
    {
        deathScreen.SetActive(false);

        if (jumpscareEnemy != null)
        {
            jumpscareEnemy.SetActive(false);
            rbJumpscareEnemy = jumpscareEnemy.GetComponent<Rigidbody>();
        }

        rbPlayer = playerCamera.GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        // Força a câmera olhar para o inimigo
        if (forceLook && lookTarget != null)
        {
            Vector3 dir = (lookTarget.position - playerCamera.transform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir);

            playerCamera.transform.rotation = Quaternion.Slerp(
                playerCamera.transform.rotation,
                targetRot,
                Time.deltaTime * lookSpeed
            );
        }

        // Congela posição do player
        if (lockPosition)
        {
            Transform root = playerCamera.transform.root;
            root.position = frozenPosition;
        }

        // Congela posição do inimigo
        if (freezeEnemyTransform && jumpscareEnemy != null)
        {
            jumpscareEnemy.transform.position = frozenEnemyPos;
        }
    }

    public void TriggerJumpscare()
    {
        if (triggered) return;
        triggered = true;

        PlayerLock.IsLocked = true;

        // Congela inimigo normal (se tiver IA)
        if (normalEnemy != null)
            normalEnemy.SendMessage("FreezeEnemy", SendMessageOptions.DontRequireReceiver);

        // Congela o player
        if (rbPlayer != null)
        {
            rbPlayer.isKinematic = true;
            rbPlayer.linearVelocity = Vector3.zero;
            rbPlayer.angularVelocity = Vector3.zero;
        }

        frozenPosition = playerCamera.transform.root.position;
        lockPosition = true;

        // Ativa inimigo de jumpscare
        jumpscareEnemy.SetActive(true);

        // Congela inimigo na frente do player
        FreezeJumpscareEnemy();

        // Força a câmera olhar
        forceLook = true;

        // Som
        if (jumpscareSound != null)
            jumpscareSound.Play();

        // Tela de morte
        Invoke(nameof(ShowDeathScreen), 0.8f);
    }

    // -----------------------------
    // FUNÇÃO: Congelar inimigo
    // -----------------------------
    private void FreezeJumpscareEnemy()
    {
        // 1 - Teleportar inimigo para frente do player
        jumpscareEnemy.transform.position = GetFreezePositionInFrontOfPlayer();
        frozenEnemyPos = jumpscareEnemy.transform.position;

        // 2 - Congelar Rigidbody
        if (rbJumpscareEnemy != null)
        {
            rbJumpscareEnemy.isKinematic = true;
            rbJumpscareEnemy.linearVelocity = Vector3.zero;
            rbJumpscareEnemy.angularVelocity = Vector3.zero;
        }

        // 3 - Desativar NavMeshAgent
        NavMeshAgent agent = jumpscareEnemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        // 4 - Desativar todos os scripts do inimigo (exceto este)
        MonoBehaviour[] scripts = jumpscareEnemy.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script != this)
                script.enabled = false;
        }

        // 5 - Travar transform
        freezeEnemyTransform = true;
    }

    // ---------------------------------------------------------------
    // Calcula a posição do inimigo na frente do player
    // ---------------------------------------------------------------
    private Vector3 GetFreezePositionInFrontOfPlayer()
    {
        Transform playerRoot = playerCamera.transform.root;
        Vector3 pos = playerRoot.position + playerRoot.forward * freezeDistanceFromPlayer;
        return pos;
    }

    private void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}