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

    public float hideLookSensitivity = 2f;

    private PlayerItemPickup itemPickup;
    private Transform holdPoint;

    private Vector3 savedLocalPos;
    private Quaternion savedLocalRot;

    void Start()
    {
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

        bool lookingAtHideSpot = false;

        if (!isHiding && playerCamera != null)
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, 3f, interactMask))
            {
                if (hit.collider != null && hit.collider.transform == transform)
                    lookingAtHideSpot = true;
            }
        }

        if (interactPrompt != null)
        {
            if (!isHiding && lookingAtHideSpot && dist <= interactDistance)
                interactPrompt.SetActive(true);
            else
                interactPrompt.SetActive(false);
        }

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

        if (playerCamera != null)
            playerCamera.gameObject.SetActive(false);
        hideCamera.gameObject.SetActive(true);

        if (controller != null)
            controller.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        PlayerHiddenState.isHidden = true;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (holdPoint != null)
        {
            savedLocalPos = holdPoint.localPosition;
            savedLocalRot = holdPoint.localRotation;

            holdPoint.localPosition = savedLocalPos;
            holdPoint.localRotation = savedLocalRot;
        }

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

        if (holdPoint != null && playerCamera != null)
        {
            holdPoint.localPosition = savedLocalPos;
            holdPoint.localRotation = savedLocalRot;
        }

        if (itemPickup != null && itemPickup.heldItem != null)
            itemPickup.heldItem.gameObject.SetActive(true);
    }
}