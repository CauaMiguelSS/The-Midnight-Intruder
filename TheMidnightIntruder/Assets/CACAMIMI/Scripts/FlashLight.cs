using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    private bool FlashLightActive = false;

    void Start()
    {
        FlashlightLight.SetActive(false);
    }

    void Update()
    {
        if (!KeyManager.Instance.lanterna)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            FlashLightActive = !FlashLightActive;
            FlashlightLight.SetActive(FlashLightActive);
        }
    }
}

