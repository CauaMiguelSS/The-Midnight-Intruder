using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public string requiredKeyID;
    public GameObject cadeado;
    public Animator doorAnimator; 

    public AudioSource doorAudioSource;
    public AudioClip openSound;
    public AudioClip lockAttemptSound;
    
    private bool isLocked = true; 

    public void TryUnlock(KeyItem keyHeld, PlayerItemPickup player)
    {
        if (!isLocked) 
        {
            return;
        }

        if (keyHeld == null || keyHeld.keyID != requiredKeyID)
        {
            PlayOpenSound(lockAttemptSound); 
            
            Debug.LogWarning("Porta trancada. Chave incorreta ou faltando.");
        }
        else
        {
            UnlockDoor(keyHeld, player);
        }
    }

    void UnlockDoor(KeyItem key, PlayerItemPickup player)
    {
        isLocked = false;
    
        if (cadeado != null)
            Destroy(cadeado);

        PlayOpenSound(openSound);

        if (doorAnimator != null)
            doorAnimator.SetTrigger("Open");

        player.DropItem(); 
        Destroy(key.gameObject); 
    }

    void PlayOpenSound(AudioClip clipToPlay)
    {
        if (doorAudioSource != null && clipToPlay != null)
        {
            doorAudioSource.clip = clipToPlay;
            doorAudioSource.Play();
        }
    }
}