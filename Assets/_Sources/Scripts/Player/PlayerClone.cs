using UnityEngine;
using UnityEngine.AI;

public class PlayerClone : MonoBehaviour
{
    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 20f;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayerVisual visual;

    void Awake()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void Initialize(Vector3 startPos)
    {
        agent.Warp(startPos);
        agent.isStopped = false;
        GetRandomPos();
    }

    void Update()
    {
        bool isRunning = agent.enabled && !agent.pathPending && agent.remainingDistance > agent.stoppingDistance;
        var direction = ((Vector2)agent.velocity).normalized;

        visual.SetRunning(isRunning);
        visual.SetDirection(direction);
    }

    private void GetRandomPos()
    {
        var randomDirection = Random.insideUnitCircle.normalized * Random.Range(minDistance, maxDistance);
        var candidate = (Vector2) transform.position + randomDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(candidate, out hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            GetRandomPos();
        }
    }

    public void Disable()
    {
        agent.isStopped = true;
        gameObject.SetActive(false);
    }
}
