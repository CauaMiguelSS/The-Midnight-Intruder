using UnityEngine;

public class PlayerCTRL : MonoBehaviour
{
    public float movSpeed;
    float speedX, speedY;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (DialogueSystem.Instance != null && DialogueSystem.Instance.DialogueAtivo())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movSpeed;

        rb.linearVelocity = new Vector2(speedX, speedY);
    }
}
