using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public string requiredKeyID;
    public GameObject cadeado;      // opcional
    public Animator doorAnimator;   // animação de abrir porta

    public void TryUnlock(KeyItem key)
    {
        if (key.keyID == requiredKeyID)
        {
            UnlockDoor();
            Destroy(key.gameObject); // chave desaparece após uso
        }
    }

    void UnlockDoor()
    {
        if (cadeado != null)
            Destroy(cadeado);

        if (doorAnimator != null)
            doorAnimator.SetTrigger("Open");
    }
}
