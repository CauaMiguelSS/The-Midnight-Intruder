using UnityEngine;
using UnityEngine.AI;

public class EnemyFootsteps : MonoBehaviour
{
    public AudioSource footstepAudio;
    public Transform player;

    [Header("Configuração de distância")]
    public float maxDistance = 20f;   // distância máxima para ficar audível

    [Header("Volume")]
    public float minVolume = 0.05f;
    public float maxVolume = 0.8f;

    void Start()
    {
        if (footstepAudio != null)
        {
            footstepAudio.loop = true;
            footstepAudio.volume = minVolume;
            footstepAudio.Play();
        }
    }

    void Update()
    {
        if (footstepAudio == null || player == null)
            return;

        // Distância do player
        float distance = Vector3.Distance(transform.position, player.position);

        // Normaliza a distância (0 = perto, 1 = longe)
        float t = Mathf.Clamp01(1 - (distance / maxDistance));

        // Volume baseado na distância
        footstepAudio.volume = Mathf.Lerp(minVolume, maxVolume, t);
    }
}