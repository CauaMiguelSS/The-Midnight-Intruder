using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrol, Investigate, Chase, Search }
    public EnemyState currentState;

    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;
    public Transform[] patrolPoints;

    [Header("Vision Settings")]
    public float viewDistance = 10f;
    public float viewAngle = 90f;
    public LayerMask obstacleMask;

    [Header("Hearing Settings")]
    public float hearingRange = 12f;
    private Vector3 noisePosition;
    private bool heardNoise;

    [Header("Search Settings")]
    public float searchDuration = 8f;
    private float searchTimer;

    private int patrolIndex = 0;
    private Vector3 lastSeenPosition;

    public JumpscareController jumpscareManager; // arraste no inspector
    public float triggerDistance = 1.5f;
    private bool jumpscareTriggered = false;

    void Start()
    {
        currentState = EnemyState.Patrol;
        agent.SetDestination(patrolPoints[patrolIndex].position);
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Investigate:
                Investigate();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Search:
                Search();
                break;
        }

        DetectPlayer();
    }

    // ========== DETECÇÃO ==========
    void DetectPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);

        bool inViewDistance = distance <= viewDistance;
        bool inViewAngle = Vector3.Angle(transform.forward, dir) < viewAngle / 2;

        if (inViewDistance && inViewAngle)
        {
            if (!Physics.Raycast(transform.position + Vector3.up * 1.5f, dir, distance, obstacleMask))
            {
                currentState = EnemyState.Chase;
                lastSeenPosition = player.position;
            }
        }
    }

    // Chamado por outros scripts para gerar barulho
    public void HearNoise(Vector3 pos)
    {
        if (Vector3.Distance(transform.position, pos) <= hearingRange)
        {
            noisePosition = pos;
            heardNoise = true;
            currentState = EnemyState.Investigate;
        }
    }

    // ========== ESTADOS ==========

    void Patrol()
    {
        if (agent.remainingDistance < 0.5f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[patrolIndex].position);
        }

        if (heardNoise)
        {
            currentState = EnemyState.Investigate;
        }
    }

    void Investigate()
    {
        agent.SetDestination(noisePosition);

        if (Vector3.Distance(transform.position, noisePosition) < 1f)
        {
            heardNoise = false;
            searchTimer = 0;
            currentState = EnemyState.Search;
        }
    }

    void Chase()
    {
        if (jumpscareTriggered) return;

        agent.SetDestination(player.position);
        lastSeenPosition = player.position;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= triggerDistance)
        {
            jumpscareTriggered = true;
            jumpscareManager.TriggerJumpscare();
        }

        // fallback: se o player sumir, mudar de estado
        if (dist > viewDistance * 1.5f)
        {
            currentState = EnemyState.Search;
            searchTimer = 0;
        }
    }

    void Search()
    {
        agent.SetDestination(lastSeenPosition);

        if (agent.remainingDistance < 1f)
        {
            searchTimer += Time.deltaTime;

            if (searchTimer >= searchDuration)
            {
                heardNoise = false;
                currentState = EnemyState.Patrol;
                agent.SetDestination(patrolPoints[patrolIndex].position);
            }
        }
    }

    public void FreezeEnemy()
    {
        // Se estiver usando NavMeshAgent — para o movimento
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        // Se tiver animação de andar
        var animator = GetComponent<Animator>();
        if (animator != null)
            animator.speed = 0;

        // Se tiver script de movimento próprio
        this.enabled = false;
    }
}
