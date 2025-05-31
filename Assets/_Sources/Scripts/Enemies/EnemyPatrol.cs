using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//TODO: Maybe creates a EnemyPatrol if gets too big
public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private bool waitToStartPatrol;
    [SerializeField][Min(3)] private float rangePatrol = 5f;
    [SerializeField] private float rangeToEnterChase = 3f;
    [SerializeField] private float rangeToOutChase = 4f;
    [SerializeField] private float rangeAttack = 1f;
    [SerializeField] private float stoppedTime = 2f;

    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private EnemyVisual visual;

    private Vector2 NavMeshTarget;
    private bool isWaitingNewTarget = false;

    public bool IsAttacking { get; private set; }
    public Transform TargetFind { get; private set; }
    
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (!TargetFind)
        {
            Patrolling();
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
        LookingForTarget();
    }

    private void Patrolling()
    {
        if (waitToStartPatrol) return;
        if (waitToStartPatrol) return;

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
        NavMeshTarget = TargetFind.position;

        if (!IsInsideRange(rangeToOutChase))
        {
            agent.isStopped = true;
            TargetFind = null;
            UpdateRandomPoint();

            await Task.Delay((int)(stoppedTime * 1000));
            agent.isStopped = false;
        }
    }

    private void LookingForTarget()
    {
        var player = Physics2D.OverlapCircle(transform.position, rangeToEnterChase, 1 << 6);

        if (player != null)
        {
            TargetFind = player.transform;
            waitToStartPatrol = false;
            return;
        }

        var npc = Physics2D.OverlapCircle(transform.position, rangeToEnterChase, 1 << 7);

        if (npc != null)
            TargetFind = npc.transform;
    }

    private void MoveToNavMeshTarget()
    {
        if (waitToStartPatrol) return;
        
        visual.SetRunning(!agent.isStopped);
        visual.SetDirection(NavMeshTarget);

        agent.SetDestination(NavMeshTarget);
    }

    private void AttackTarget()
    {
        if (!agent.isStopped) agent.isStopped = true;
        IsAttacking = true;
    }

    public void StopAttack()
    {
        TargetFind = null;
        agent.isStopped = false;
        IsAttacking = false;
    }

    private bool IsInsideRange(float range)
    {
        var srqDist = (transform.position - TargetFind.position).sqrMagnitude;

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
