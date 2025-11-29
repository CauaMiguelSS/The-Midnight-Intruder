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
        
        // Destruição da chave
        player.DropItem(); 
        Destroy(key.gameObject); 
    }

    void PlayOpenSound(AudioClip clipToPlay)
    {
        // Verifica se o AudioSource existe e se o clip foi fornecido
        if (doorAudioSource != null && clipToPlay != null)
        {
            // Define o clip e toca
            doorAudioSource.clip = clipToPlay;
            doorAudioSource.Play();
        }
    }
}