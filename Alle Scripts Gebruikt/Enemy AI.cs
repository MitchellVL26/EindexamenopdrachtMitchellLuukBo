using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;   // assign in inspector
    public float detectionRadius = 10f;

    private int currentPatrolIndex;
    private NavMeshAgent agent;
    private Transform player;
    private EnemyShooting shooting;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        shooting = GetComponent<EnemyShooting>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (patrolPoints.Length > 0)
            GoToNextPatrolPoint();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= detectionRadius)
        {
            // chase player
            agent.SetDestination(player.position);

            // enable shooting
            if (shooting != null && !shooting.enabled)
                shooting.enabled = true;
        }
        else
        {
            // patrol
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GoToNextPatrolPoint();

            // disable shooting
            if (shooting != null && shooting.enabled)
                shooting.enabled = false;
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}