using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    public float movSpeed;

    float speedX, speedY;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogWarning("PlayerMoviment: Rigidbody2D n�o encontrado!");
    }

    void Update()
    {
        
        if (DialogueSystem.Instance != null && DialogueSystem.Instance.DialogueAtivo())
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
            return;
        }

        speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movSpeed;

        if (rb != null)
            rb.linearVelocity = new Vector2(speedX, speedY);
    }
}

