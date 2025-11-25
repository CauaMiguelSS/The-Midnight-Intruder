using UnityEngine;

public class HideSpot_3D : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private Transform player;
    [SerializeField] private Camera hideCamera;
    [SerializeField] private float interactDistance = 2.0f;

    private Camera playerCamera;
    private Controller3D controller;
    private Rigidbody rb;

    private bool isHiding = false;

    void Start()
    {
        controller = player.GetComponent<Controller3D>();
        rb = player.GetComponent<Rigidbody>();
        playerCamera = player.GetComponentInChildren<Camera>();

        hideCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= interactDistance && Input.GetKeyDown(interactKey))
        {
            if (!isHiding) EnterHide();
            else ExitHide();
        }
    }

    void EnterHide()
    {
        isHiding = true;

       
        playerCamera.gameObject.SetActive(false);
        hideCamera.gameObject.SetActive(true);

        
        controller.enabled = false;

        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void ExitHide()
    {
        isHiding = false;

        hideCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);

        controller.enabled = true;
    }
}


