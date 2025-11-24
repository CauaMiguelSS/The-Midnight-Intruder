using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class GerenciadorSomCaixas: MonoBehaviour
{
    public AudioClip slidingSound;
    public float velocityThreshold = 0.05f;

    private Rigidbody2D rb;
    private AudioSource audioSrc;
    private bool playerTouching = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        audioSrc = gameObject.AddComponent<AudioSource>();
        audioSrc.clip = slidingSound;
        audioSrc.loop = true;
        audioSrc.playOnAwake = false;
        audioSrc.volume = 0.3f;

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerTouching = true;

            // velocidade da caixa
            float vel = rb.linearVelocity.magnitude;

            if (vel > velocityThreshold)
            {
                if (!audioSrc.isPlaying)
                    audioSrc.Play();
            }
            else
            {
                if (audioSrc.isPlaying)
                    audioSrc.Stop();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerTouching = false;
            audioSrc.Stop();
        }
    }
}
