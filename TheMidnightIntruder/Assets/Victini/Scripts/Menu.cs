using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    private bool isMenuOpen = false;
    private bool cameraLock = true;

    Controller3D controller3D;

    public void Start()
    {
        controller3D = FindAnyObjectByType(typeof(Controller3D)) as Controller3D;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        cameraLock = !cameraLock;
        isMenuOpen = !isMenuOpen;
        menuPanel.SetActive(isMenuOpen);

        controller3D.enabled = cameraLock;

        Time.timeScale = isMenuOpen ? 0f : 1f;

        Cursor.visible = isMenuOpen;
        Cursor.lockState = isMenuOpen ? CursorLockMode.None : CursorLockMode.Locked;

        GamePauseState.isPaused = isMenuOpen;
    }
}