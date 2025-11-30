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

    // Suavidade (agora aplicadas no FixedUpdate)
    public float moveSpeed = 20f;
    public float rotateSpeed = 20f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        originalScale = transform.localScale;
        originalParent = transform.parent;
    }

    void FixedUpdate()
    {
        if (!beingHeld || holdPoint == null) return;

        // -------- MOVIMENTO SUAVE FÍSICO --------
        Vector3 targetPos = holdPoint.position;
        Vector3 newPos = Vector3.Lerp(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        // -------- ROTAÇÃO SUAVE FÍSICA --------
        Quaternion targetRot = holdPoint.rotation;
        Quaternion newRot = Quaternion.Lerp(rb.rotation, targetRot, rotateSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(newRot);
    }

    public virtual void OnPickUp(Transform holdPoint)
    {
        this.holdPoint = holdPoint;
        beingHeld = true;

        // Física mais controlada (não usar isKinematic)
        rb.useGravity = false;
        rb.linearDamping = 10f;
        rb.angularDamping = 10f;

        col.enabled = false; // evita colisão com o player
    }

    public virtual void OnDrop()
    {
        beingHeld = false;
        holdPoint = null;

        // Soltar física
        rb.useGravity = true;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;

        col.enabled = true;

        // Volta escala original
        transform.localScale = originalScale;
    }
}