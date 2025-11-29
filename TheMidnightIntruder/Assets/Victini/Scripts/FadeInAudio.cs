using UnityEngine;

public class FadeInAudio : MonoBehaviour
{
    [Tooltip("Duração (em segundos) para o som atingir o volume máximo.")]
    public float fadeInDuration = 10f; 

    [Tooltip("Volume final máximo que o som deve atingir (ex: 1.0).")]
    [Range(0f, 1f)]
    public float targetVolume = 1f;

    private AudioSource audioSource;
    private float timeElapsed = 0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.volume = 0f;

        audioSource.Play();
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        float volumeRatio = timeElapsed / fadeInDuration;

        if (volumeRatio <= 1f)
        {
            audioSource.volume = Mathf.Lerp(0f, targetVolume, volumeRatio);
        }
        else
        {
            audioSource.volume = targetVolume;
            enabled = false;
        }
    }
}