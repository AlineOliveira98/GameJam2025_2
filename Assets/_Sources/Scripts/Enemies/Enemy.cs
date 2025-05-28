using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//TODO: Maybe creates a EnemyPatrol if gets too big
public class Enemy : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField][Min(3)] private float rangePatrol = 5f;
    [SerializeField] private float rangeToEnterChase = 3f;
    [SerializeField] private float rangeToOutChase = 4f;
    [SerializeField] private float stoppedTime = 2f;

    [Header("Attack")]
    [SerializeField] private float damage;
    [SerializeField] private float attackRate;
    [SerializeField] private float rangeAttack = 1f;


    [Header("References")]
    [SerializeField] private NavMeshAgent agent;

    private Transform targetFind;
    private Vector2 NavMeshTarget;
    private bool isWaitingNewTarget = false;
    private float lastAttackTime = -Mathf.Infinity;

    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (!targetFind)
        {
            Patrolling();
            LookingForTarget();
        }
        else if (IsInsideRange(rangeAttack))
        {
            AttackTarget();
        }
        else
        {
            Chase();
        }

        MoveToNavMeshTarget();
    }

    private void Patrolling()
    {
        if (isWaitingNewTarget) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            WaitAndSetNewPatrolPoint();
        }
    }

    private async void WaitAndSetNewPatrolPoint()
    {
        agent.isStopped = true;

        isWaitingNewTarget = true;
        UpdateRandomPoint();
        await Task.Delay((int)(stoppedTime * 1000));
        agent.isStopped = false;

        isWaitingNewTarget = false;
    }

    private async void Chase()
    {
        agent.isStopped = false;
        NavMeshTarget = targetFind.position;

        if (!IsInsideRange(rangeToOutChase))
        {
            agent.isStopped = true;
            targetFind = null;
            UpdateRandomPoint();

            await Task.Delay((int)(stoppedTime * 1000));
            agent.isStopped = false;
        }
    }

    private void LookingForTarget()
    {
        var target = Physics2D.OverlapCircle(transform.position, rangeToEnterChase, 1 << 6 | 1 << 7);

        if (target != null) targetFind = target.transform;
    }

    private void MoveToNavMeshTarget()
    {
        agent.SetDestination(NavMeshTarget);
    }

    private void AttackTarget()
    {
        if (!agent.isStopped) agent.isStopped = true;

        if (Time.time >= lastAttackTime + attackRate)
        {
            if (targetFind.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }

            lastAttackTime = Time.time;
        }
    }

    private bool IsInsideRange(float range)
    {
        var srqDist = (transform.position - targetFind.position).sqrMagnitude;

        return srqDist <= range * range;
    }

    private void UpdateRandomPoint()
    {
        bool isInsideNavMesh;

        while (true)
        {
            Vector2 randomPoint = (Vector2)transform.position + Random.insideUnitCircle * rangePatrol;
            isInsideNavMesh = NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas);

            if (isInsideNavMesh)
            {
                NavMeshTarget = hit.position;
                break;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeAttack);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeToEnterChase);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangeToOutChase);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangePatrol);
    }
}
