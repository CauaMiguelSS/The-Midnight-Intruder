using UnityEngine;
using UnityEngine.AI;

public class EnemyFootsteps : MonoBehaviour
{
    public AudioSource footstepAudio;
    public NavMeshAgent agent;
    public Transform player;
    public float maxDistance = 20f;   // distancia máxima para ouvir
    public float minVolume = 0.05f;
    public float maxVolume = 0.8f;

    void Start()
    {
        if (footstepAudio != null)
            footstepAudio.Play(); // começa já tocando em loop
    }

    void Update()
    {
        if (footstepAudio == null || agent == null || player == null)
            return;

        // Se o inimigo estiver parado, volume 0
        if (agent.velocity.magnitude < 0.1f)
        {
            footstepAudio.volume = 0f;
            return;
        }

        // Calcula distância até o player
        float distance = Vector3.Distance(transform.position, player.position);

        // Faz volume baseado na distância
        float t = Mathf.Clamp01(1 - (distance / maxDistance));
        footstepAudio.volume = Mathf.Lerp(minVolume, maxVolume, t);
    }
}