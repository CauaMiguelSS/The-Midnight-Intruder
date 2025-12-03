using UnityEngine;

public class PickUpItem : MonoBehaviour

{
    public string itemName; 
    private bool playerPerto = false;

    [Header("Audio")]
    public AudioClip pickupSound;  
    public AudioSource audioSource; 

    void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E))
        {
            KeyManager.Instance.AddItem(itemName);

            if (itemName == "Lanterna" && audioSource && pickupSound)
                audioSource.PlayOneShot(pickupSound);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = false;
        }
    }
}