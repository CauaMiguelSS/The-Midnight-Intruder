using UnityEngine;

public class Item : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public virtual void OnPickUp(Transform holdPoint)
    {
        rb.isKinematic = true;
        col.enabled = false;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public virtual void OnDrop()
    {
        transform.SetParent(null);

        rb.isKinematic = false;
        col.enabled = true;
    }
}

