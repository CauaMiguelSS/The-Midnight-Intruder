using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class Controller3D : MonoBehaviour
{
    [Header("Movement Settings:")]
    [Range(0.01f, 20.0f)][SerializeField] private float m_walkSpeed = 5.0f;
    [Range(0.01f, 20.0f)][SerializeField] private float m_sprintSpeed = 8.0f;

    [Header("Camera Settings")]
    [Range(0.01f, 10.0f)][SerializeField] private float m_mouseSensitivity = 5f;
    [SerializeField] private float m_minimumEulerAngle = -60.0f;
    [SerializeField] private float m_maximumEulerAngle = 60.0f;
    private float m_cameraVerticalRotation = 0.0f;

    private Rigidbody m_rigidbody = null;
    private Camera m_camera = null;

    private float Velocity => Input.GetKey(KeyCode.LeftShift) ? m_sprintSpeed : m_walkSpeed;
    private float HorizontalAxis => Input.GetAxis("Horizontal");
    private float VerticalAxis => Input.GetAxis("Vertical");
    private float MouseX => Input.GetAxis("Mouse X");
    private float MouseY => Input.GetAxis("Mouse Y");

    private void Awake()
    {
        m_camera = GetComponentInChildren<Camera>();
        m_rigidbody = GetComponent<Rigidbody>();

        m_rigidbody.freezeRotation = true;
        m_rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void Update()
    {
        
        transform.Rotate(Vector3.up * MouseX * m_mouseSensitivity);
        
        m_cameraVerticalRotation -= MouseY * m_mouseSensitivity;
        m_cameraVerticalRotation = Mathf.Clamp(m_cameraVerticalRotation, m_minimumEulerAngle, m_maximumEulerAngle);
        m_camera.transform.localRotation = Quaternion.Euler(m_cameraVerticalRotation, 0f, 0f);
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = (transform.forward * VerticalAxis + transform.right * HorizontalAxis).normalized;
        m_rigidbody.linearVelocity = moveDirection * Velocity + new Vector3(0, m_rigidbody.linearVelocity.y, 0);
    }
}
