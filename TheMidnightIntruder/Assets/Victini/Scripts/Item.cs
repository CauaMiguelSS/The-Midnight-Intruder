using UnityEngine;

public class Item : MonoBehaviour
{
    protected Rigidbody rb;
    protected Collider col;

    protected Transform holdPoint;
    protected bool beingHeld = false;

    // Guarda valores originais
    protected Vector3 originalScale;
    protected Transform originalParent;

    // Suavidade
    public float moveSpeed = 15f;
    public float rotateSpeed = 15f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        originalScale = transform.localScale;
        originalParent = transform.parent;
    }

    protected virtual void Update()
    {
        if (beingHeld && holdPoint != null)
        {
            // mover suavemente
            transform.position = Vector3.Lerp(
                transform.position,
                holdPoint.position,
                Time.deltaTime * moveSpeed
            );

            // girar suave
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                holdPoint.rotation,
                Time.deltaTime * rotateSpeed
            );
        }
    }

    public virtual void OnPickUp(Transform holdPoint)
    {
        this.holdPoint = holdPoint;
        beingHeld = true;

        // remover física para não “brigar” com a mão
        rb.isKinematic = true;
        rb.useGravity = false;
        col.enabled = false;
    }

    public virtual void OnDrop()
    {
        beingHeld = false;
        holdPoint = null;

        // soltar física
        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;

        // voltar escala original se algo mudou
        transform.localScale = originalScale;
    }
}