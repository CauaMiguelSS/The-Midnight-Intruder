using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
