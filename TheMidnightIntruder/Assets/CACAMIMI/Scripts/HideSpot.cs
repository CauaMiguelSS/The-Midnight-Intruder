using UnityEngine;
using UnityEngine.UI; // Para o UI Text

public class HideSpot_3D : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private Transform player;
    [SerializeField] private Camera hideCamera;
    [SerializeField] private float interactDistance = 2.0f;

    [Header("UI Prompt")]
    [SerializeField] private GameObject interactPrompt; // Objeto UI "Aperte E para se esconder"

    private Camera playerCamera;
    private Controller3D controller;
    private Rigidbody rb;

    private bool isHiding = false;
    private bool mouseOver = false; // controla se o mouse está sobre o HideSpot

    void Start()
    {
        controller = player.GetComponent<Controller3D>();
        rb = player.GetComponent<Rigidbody>();
        playerCamera = player.GetComponentInChildren<Camera>();

        hideCamera.gameObject.SetActive(false);

        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        // Mostrar ou esconder o prompt
        if (interactPrompt != null)
        {
            if (!isHiding && mouseOver && dist <= interactDistance)
                interactPrompt.SetActive(true);
            else
                interactPrompt.SetActive(false);
        }

        // Interação
        if (dist <= interactDistance && Input.GetKeyDown(interactKey))
        {
            if (!isHiding && mouseOver)
                EnterHide();
            else if (isHiding)
                ExitHide();
        }
    }

    // Detecta quando o mouse entra no collider
    void OnMouseEnter()
    {
        mouseOver = true;
    }

    // Detecta quando o mouse sai do collider
    void OnMouseExit()
    {
        mouseOver = false;
    }

    void EnterHide()
    {
        isHiding = true;

        playerCamera.gameObject.SetActive(false);
        hideCamera.gameObject.SetActive(true);

        controller.enabled = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Esconde o prompt enquanto está escondido
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    void ExitHide()
    {
        isHiding = false;

        hideCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);

        controller.enabled = true;
    }
}