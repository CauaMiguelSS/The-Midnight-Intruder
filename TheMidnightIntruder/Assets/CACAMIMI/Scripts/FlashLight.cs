using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    private bool FlashLightActive = false;

    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip toggleSound; // som de click ao ligar/desligar

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

            // Toca o som de click
            if (audioSource && toggleSound)
                audioSource.PlayOneShot(toggleSound);
        }
    }
}

