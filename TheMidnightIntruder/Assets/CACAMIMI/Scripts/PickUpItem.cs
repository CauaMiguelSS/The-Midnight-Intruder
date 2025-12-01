using UnityEngine;

public class PickUpItem : MonoBehaviour

{
    public string itemName; // Nome do item (Chave1, Chave2, Chave3, Martelo)
    private bool playerPerto = false;

    [Header("Audio")]
    public AudioClip pickupSound;   // som ao pegar
    public AudioSource audioSource; // pode ser do Player

    void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E))
        {
            KeyManager.Instance.AddItem(itemName);

            // Se for a lanterna, toca o som
            if (itemName == "Lanterna" && audioSource && pickupSound)
                audioSource.PlayOneShot(pickupSound);

            Destroy(gameObject); // remove a lanterna da bancada
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