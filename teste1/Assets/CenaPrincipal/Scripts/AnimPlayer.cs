using UnityEngine;

public class AnimPlayer : MonoBehaviour
{
    public Animator anim;
    public float speed;

    void Update()
    {
        
        if (DialogueSystem.Instance != null && DialogueSystem.Instance.DialogueAtivo())
        {
            anim.SetFloat("Horizontal", 0f);
            anim.SetFloat("Vertical", 0f);
            anim.SetFloat("Speed", 0f);
            return; 
        }

        
        Vector3 movement = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"),
            0f
        );

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.magnitude);

        transform.position += movement * speed * Time.deltaTime;
    }
}

