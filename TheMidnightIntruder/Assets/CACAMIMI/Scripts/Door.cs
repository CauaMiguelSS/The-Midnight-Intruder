using UnityEngine;

public class Door : MonoBehaviour
{
    public string requiredItem;
    private bool playerPerto = false;

    void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E))
        {
            if (KeyManager.Instance.HasItem(requiredItem))
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Você não tem o item necessário: " + requiredItem);
            }
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


