using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    private bool FlashLightActive = false;
    void Start()
    {
        FlashlightLight.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (FlashLightActive == false)
            {
                FlashlightLight.gameObject.SetActive(true);
                FlashLightActive = true;
            }

            else 
            {
                FlashlightLight.gameObject.SetActive(false);
                FlashLightActive = false;
            }

        }
    }
}
