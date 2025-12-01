using UnityEngine;

public class HideSpot_3D : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private Transform player;
    [SerializeField] private Camera hideCamera;
    [SerializeField] private float interactDistance = 2.0f;
    [SerializeField] private LayerMask interactMask;

    [Header("UI Prompt")]
    [SerializeField] private GameObject interactPrompt;

    private Camera playerCamera;
    private Controller3D controller;
    private Rigidbody rb;

    private bool isHiding = false;

    private Quaternion initialHideRotation;
    private float currentYaw = 0f;

    // Sensibilidade da câmera do esconderijo (corrigido aqui)
    public float hideLookSensitivity = 2f;

    // Para mexer no item enquanto esconde
    private PlayerItemPickup itemPickup;
    private Transform holdPoint;

    // Guardar local position/rotation do holdPoint
    private Vector3 savedLocalPos;
    private Quaternion savedLocalRot;

    void Start()
    {
        // segurança: checagens mínimas
        if (player == null)
        {
            Debug.LogError("[HideSpot_3D] Player não atribuído no inspector.");
            enabled = false;
            return;
        }

        controller = player.GetComponent<Controller3D>();
        rb = player.GetComponent<Rigidbody>();
        playerCamera = player.GetComponentInChildren<Camera>();
        itemPickup = player.GetComponent<PlayerItemPickup>();

        // se existe itemPickup, pega o holdPoint (senão fica nulo e checamos antes de usar)
        if (itemPickup != null)
            holdPoint = itemPickup.holdPoint;

        if (hideCamera == null)
        {
            Debug.LogError("[HideSpot_3D] hideCamera não atribuído no inspector.");
            enabled = false;
            return;
        }

        if (playerCamera == null)
        {
            Debug.LogError("[HideSpot_3D] playerCamera não encontrado como filho do player.");
            enabled = false;
            return;
        }

        initialHideRotation = hideCamera.transform.localRotation;
        hideCamera.gameObject.SetActive(false);

        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        // ---------------------------------------------------------------------
        // DETECÇÃO POR RAYCAST (olhando para o esconderijo)
        // ---------------------------------------------------------------------
        bool lookingAtHideSpot = false;

        if (!isHiding && playerCamera != null)
        {
            // usar o centro da câmera para olhar (mais consistente)
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, 3f, interactMask))
            {
                if (hit.collider != null && hit.collider.transform == transform)
                    lookingAtHideSpot = true;
            }
        }

        // UI “APERTE E”
        if (interactPrompt != null)
        {
            if (!isHiding && lookingAtHideSpot && dist <= interactDistance)
                interactPrompt.SetActive(true);
            else
                interactPrompt.SetActive(false);
        }

        // INTERAÇÃO
        if (dist <= interactDistance && Input.GetKeyDown(interactKey))
        {
            if (!isHiding && lookingAtHideSpot) EnterHide();
            else if (isHiding) ExitHide();
        }

        if (isHiding && !GamePauseState.isPaused)
        {
            float mouseX = Input.GetAxis("Mouse X") * hideLookSensitivity;
            currentYaw += mouseX;
            hideCamera.transform.localRotation =
                initialHideRotation * Quaternion.Euler(0f, currentYaw, 0f);
        }
    }

    void EnterHide()
    {
        isHiding = true;

        // troca de câmeras (playerCamera pode ser null-checked)
        if (playerCamera != null)
            playerCamera.gameObject.SetActive(false);
        hideCamera.gameObject.SetActive(true);

        // desativa movimento (se existir)
        if (controller != null)
            controller.enabled = false;

        // para drift do rigidbody (se existir)
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // avisa inimigo
        PlayerHiddenState.isHidden = true;

        // esconde prompt
        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        // se tivermos holdPoint, guardamos e reparentamos em local space
        if (holdPoint != null)
        {
            // guarda local transform relativo ao pai atual
            savedLocalPos = holdPoint.localPosition;
            savedLocalRot = holdPoint.localRotation;

            // reaplica local transform salvo (garante sem drift)
            holdPoint.localPosition = savedLocalPos;
            holdPoint.localRotation = savedLocalRot;
        }

        // oculta visual do item segurado (se houver)
        if (itemPickup != null && itemPickup.heldItem != null)
            itemPickup.heldItem.gameObject.SetActive(false);
    }

    void ExitHide()
    {
        isHiding = false;

        hideCamera.gameObject.SetActive(false);
        if (playerCamera != null)
            playerCamera.gameObject.SetActive(true);

        if (controller != null)
            controller.enabled = true;

        PlayerHiddenState.isHidden = false;

        // restaura parent do holdPoint para a camera do player, mantendo local transform
        if (holdPoint != null && playerCamera != null)
        {
            holdPoint.localPosition = savedLocalPos;
            holdPoint.localRotation = savedLocalRot;
        }

        // reativa visual do item segurado (se houver)
        if (itemPickup != null && itemPickup.heldItem != null)
            itemPickup.heldItem.gameObject.SetActive(true);
    }
}