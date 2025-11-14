using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerCrouch : MonoBehaviour
{
    [Header("Crouch Settings")]
    [SerializeField] private KeyCode m_crouchKey = KeyCode.LeftControl;
    [SerializeField] private float m_crouchHeight = 1.0f;
    [SerializeField] private float m_standHeight = 2.0f;
    [SerializeField] private float m_crouchSpeedMultiplier = 0.5f;
    [SerializeField] private float m_crouchCameraOffset = -0.5f;
    [SerializeField] private float m_transitionSpeed = 8f;

    private CapsuleCollider m_collider;
    private Controller3D m_controller;
    private Camera m_camera;
    private Rigidbody m_rigidbody; // Adicionado para manipulação de física

    private float m_defaultCameraY;
    private bool m_isCrouching = false;
    private float m_targetHeight;
    private float m_heightOffset; // Armazena a mudança de posição Y necessária

    private void Awake()
    {
        m_collider = GetComponent<CapsuleCollider>();
        m_controller = GetComponent<Controller3D>();
        m_camera = GetComponentInChildren<Camera>();
        m_rigidbody = GetComponent<Rigidbody>(); // Inicializa o Rigidbody

        m_defaultCameraY = m_camera.transform.localPosition.y;
        m_targetHeight = m_standHeight; // Inicia na altura normal
        m_collider.height = m_standHeight;
        m_collider.center = new Vector3(0, m_standHeight / 2f, 0); // Garante centro correto
    }

    private void Update()
    {
        if (Input.GetKeyDown(m_crouchKey))
            ToggleCrouch();

        // Controla a altura desejada e o movimento da câmera
        UpdateCrouchTargets();
    }

    private void FixedUpdate()
    {
        // 🚨 A lógica de transição AGORA acontece no FixedUpdate para sincronizar com o Rigidbody 🚨
        UpdateCrouchPhysics();
    }

    private void ToggleCrouch()
    {
        m_isCrouching = !m_isCrouching;

        // Ajusta velocidade do controlador
        float speed = m_isCrouching ? m_crouchSpeedMultiplier : 1.0f;
        m_controller.SetWalkSpeedMultiplier(speed);
    }

    private void UpdateCrouchTargets()
    {
        // Define as alturas alvo
        m_targetHeight = m_isCrouching ? m_crouchHeight : m_standHeight;

        // Suaviza movimento da câmera (Pode ser no Update)
        float targetCameraY = m_isCrouching ?
            m_defaultCameraY + m_crouchCameraOffset : m_defaultCameraY;

        Vector3 camPos = m_camera.transform.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetCameraY, Time.deltaTime * m_transitionSpeed);
        m_camera.transform.localPosition = camPos;
    }

    private void UpdateCrouchPhysics()
    {
        // 1. Calcula a nova altura suavizada
        float newHeight = Mathf.Lerp(m_collider.height, m_targetHeight, Time.fixedDeltaTime * m_transitionSpeed);

        // 2. Calcula a diferença de altura. Se a altura diminuiu, heightChange é negativo.
        float heightChange = newHeight - m_collider.height;

        // 3. Aplica a nova altura ao collider
        m_collider.height = newHeight;

        // 4. Ajusta o centro do collider para manter a base fixa
        m_collider.center = new Vector3(0, m_collider.height / 2f, 0);

        // 5. Ajusta a posição Y do Rigidbody para manter a base do collider no chão.
        // A mudança de posição deve ser METADE da mudança de altura.
        // Usamos MovePosition para mover o Rigidbody com segurança.
        Vector3 newPosition = m_rigidbody.position + new Vector3(0, heightChange / 2f, 0);
        m_rigidbody.MovePosition(newPosition);
    }
}