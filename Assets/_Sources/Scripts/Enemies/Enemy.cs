using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rangeAttack = 1f;
    [SerializeField] private float rangeFollow = 3f;
    [SerializeField] private float rangeToFindNewPointPatrol = 5f;

    [SerializeField] private float damage;
    [SerializeField] private float attackRate;

    [Header("References")]
    [SerializeField] private NavMeshAgent agent;

    public enum EnemyState {Patrolling, Following, Attacking}

    [Header("Debug")]
    [SerializeField] private EnemyState state;

    private Transform targetFind;
    private Vector2 navMeshTarget;

    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        navMeshTarget = GetRandomPoint();
        state = EnemyState.Patrolling;
    }

    void Update()
    {
        if (!targetFind)
        {
            Patrolling();
            LookingForTarget(); 
        }
        else if (IsInRangToAttack())
        {
            AttackTarget();
        }
        else
        {
            FollowTargetFounded();
        }

        MoveToNavMeshTarget();
    }

    private void Patrolling()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            navMeshTarget = GetRandomPoint();
        }

        state = EnemyState.Patrolling;
    }

    private void FollowTargetFounded()
    {
        navMeshTarget = targetFind.position;
        state = EnemyState.Following;
    }

    private void LookingForTarget()
    {
        var target = Physics2D.OverlapCircle(transform.position, rangeFollow, 1 << 6 | 1 << 7);

        if (target != null) targetFind = target.transform;
    }

    private void MoveToNavMeshTarget()
    {
        agent.SetDestination(navMeshTarget);
    }

    private void AttackTarget()
    {
        if (!agent.isStopped) agent.isStopped = true;

        if (targetFind.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }

        state = EnemyState.Attacking;
    }

    private bool IsInRangToAttack()
    {
        var distance = Vector2.Distance(transform.position, targetFind.position);

        return distance <= rangeAttack;
    }

    private Vector2 GetRandomPoint()
    {
        Vector2 randomPoint = (Vector2)transform.position + Random.insideUnitCircle * rangeToFindNewPointPatrol;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        Debug.Log("No patrol points found");
        return Vector2.zero;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeAttack);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeFollow);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeToFindNewPointPatrol);
    }
}
