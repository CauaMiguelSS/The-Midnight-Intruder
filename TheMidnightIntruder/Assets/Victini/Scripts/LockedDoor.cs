using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [Header("Key Settings")]
    public string requiredKeyID;

    [Header("References")]
    public GameObject cadeado;
    public Animator doorAnimator;

    [Header("Audio")]
    public AudioSource doorAudioSource;
    public AudioClip openSound;
    public AudioClip lockAttemptSound;

    private bool isLocked = true;

    // ----------------------------------------------------

    public void TryUnlock(KeyItem keyHeld, PlayerItemPickup player)
    {
        if (!isLocked)
            return;

        // Chave errada ou sem chave
        if (keyHeld == null || keyHeld.keyID != requiredKeyID)
        {
            PlaySound(lockAttemptSound);
            Debug.LogWarning("Porta trancada. Chave incorreta ou faltando.");
            return;
        }

        // Chave correta → abre
        UnlockDoor(keyHeld, player);
    }

    // ----------------------------------------------------

    void UnlockDoor(KeyItem key, PlayerItemPickup player)
    {
        isLocked = false;

        if (cadeado != null)
             Destroy(cadeado);

        PlaySound(openSound);

        if (doorAnimator != null)
             doorAnimator.SetTrigger("Open");

    // 🔥 DESATIVA O COLLIDER DA PORTA (faz o texto sumir)
        Collider col = GetComponent<Collider>();
        if (col != null)
             col.enabled = false;

    // remove chave da mão
        player.DropItem();
        Destroy(key.gameObject);
    }
    // ----------------------------------------------------

    private void PlaySound(AudioClip clip)
    {
        if (doorAudioSource != null && clip != null)
        {
            doorAudioSource.clip = clip;
            doorAudioSource.Play();
        }
    }

    // ----------------------------------------------------

    public bool IsLocked()
    {
        return isLocked;
    }
}