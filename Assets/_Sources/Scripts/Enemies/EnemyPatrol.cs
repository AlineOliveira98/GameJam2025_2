using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private bool waitToStartPatrol;
    [SerializeField][Min(3)] private float rangePatrol = 5f;
    [SerializeField] private float rangeToEnterChasePlayer = 3f;
    [SerializeField] private float rangeToEnterChaseAnimals = 2f;
    [SerializeField] private float rangeToOutChase = 4f;
    [SerializeField] private float rangeAttack = 1f;
    [SerializeField] private float stoppedTime = 2f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float dashDuration = 0.03f;
    [SerializeField] private float dashCooldown = 0.5f;

    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private EnemyVisual visual;
    [SerializeField] private Enemy enemy;
    [SerializeField] private AudioClip dashAudio;

    private Vector2 NavMeshTarget;
    private Player cachedPlayer;

    public bool IsAttacking { get; set; }
    public bool IsKnockback { get; set; }
    public Transform TargetFind { get; private set; }
    public NavMeshAgent Agent => agent;
    public float RangeAttack => rangeAttack;

    public IDash Dash { get; private set; }
    private bool CanMove => GameController.GameStarted && !GameController.GameIsOver;

    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        Dash = new BasicDash(enemy.EnemyMovement.Rig, dashSpeed, dashDuration, dashCooldown, agent);
    }

    void Update()
    {
        visual.SetDashing(Dash.IsDashing);

        if (!CanMove) return;
        if (IsKnockback) return;
        if (Dash.IsDashing) return;

        if (!TargetFind)
        {
            Patrolling();
        }
        else if (IsInsideRange(rangeAttack) || enemy.IsAttackCoroutineRunning)
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

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            WaitAndSetNewPatrolPoint();
        }
    }

    private async void WaitAndSetNewPatrolPoint()
    {
        agent.isStopped = true;

        UpdateRandomPoint();
        await UniTask.Delay((int)(stoppedTime * 1000));
        if(agent != null && agent.enabled) agent.isStopped = false;
    }

    private async void Chase()
    {
        agent.isStopped = false;
        IsAttacking = false;
        NavMeshTarget = TargetFind.position;

        if (TargetFind == null
        || (TargetFind != null && !TargetFind.gameObject.activeInHierarchy)
        || !IsInsideRange(rangeToOutChase)
        || (cachedPlayer != null && TargetFind == cachedPlayer.transform && cachedPlayer.IsInvisible))
        {
            agent.isStopped = true;
            TargetFind = null;
            UpdateRandomPoint();

            await UniTask.Delay((int)(stoppedTime * 1000));
            if(agent != null && agent.enabled) agent.isStopped = false;

            return;
        }

        if (Dash.CanDash && !IsKnockback)
        {
            var randomDash = Random.Range(0, 2);
            if (randomDash == 0)
            {
                Vector2 dashDirection = (TargetFind.position - transform.position).normalized;
                Dash.TryDash(dashDirection);
                visual.TriggerDash();
                AudioController.PlaySFX(dashAudio);
            }
            else
            {
                Dash.CooldownDash();
            }
        }
    }

    private void LookingForTarget()
    {
        Collider2D cloneCol = Physics2D.OverlapCircle(transform.position, rangeToEnterChasePlayer, 1 << 10);

        if (cloneCol != null)
        {
            TargetFind = cloneCol.transform;
            waitToStartPatrol = false;
            return; 
        }

        Collider2D playerCol = Physics2D.OverlapCircle(transform.position, rangeToEnterChasePlayer, 1 << 6);
        Collider2D npc = Physics2D.OverlapCircle(transform.position, rangeToEnterChaseAnimals, 1 << 7);

        float playerDist = float.MaxValue;
        float npcDist = float.MaxValue;

        if (playerCol != null)
        {
            if (cachedPlayer == null || cachedPlayer.gameObject != playerCol.gameObject)
                cachedPlayer = playerCol.GetComponent<Player>();

            playerDist = (playerCol.transform.position - transform.position).sqrMagnitude;
        }

        if (npc != null)
            npcDist = (npc.transform.position - transform.position).sqrMagnitude;

        if (playerCol != null && playerDist <= npcDist && cachedPlayer != null && !cachedPlayer.IsInvisible)
        {
            TargetFind = playerCol.transform;
            waitToStartPatrol = false;
        }
        else if (npc != null)
        {
            TargetFind = npc.transform;
            waitToStartPatrol = false;
        }
    }

    private void MoveToNavMeshTarget()
    {
        if (waitToStartPatrol) return;
        if (agent == null || !agent.enabled) return;

        visual.SetRunning(!agent.isStopped && !Dash.IsDashing);
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeAttack);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeToEnterChasePlayer);
        Gizmos.DrawWireSphere(transform.position, rangeToEnterChaseAnimals);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangeToOutChase);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangePatrol);

        #if UNITY_EDITOR
        Vector3 screenPosDet = HandleUtility.WorldToGUIPoint(transform.position + Vector3.right * rangeToEnterChasePlayer);
        Vector3 screenPosAni = HandleUtility.WorldToGUIPoint(transform.position + Vector3.right * rangeToEnterChaseAnimals);
        Vector3 screenPosAtk = HandleUtility.WorldToGUIPoint(transform.position + Vector3.right * rangeToOutChase);
        Vector3 screenPosAlt = HandleUtility.WorldToGUIPoint(transform.position + Vector3.right * rangeAttack);
        Vector3 screenPosPatrol = HandleUtility.WorldToGUIPoint(transform.position + Vector3.right * rangePatrol);

        Handles.BeginGUI();

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.normal.textColor = Color.black; 
        style.fontStyle = FontStyle.Bold;
        style.alignment = TextAnchor.MiddleCenter;
        style.padding = new RectOffset(4, 4, 2, 2);
        style.normal.background = Texture2D.whiteTexture; 

        GUI.backgroundColor = new Color(1f, 1f, 1f, 1f);

        GUI.Label(new Rect(screenPosAlt.x - 40, screenPosAlt.y - 10, 80, 20), "Attack", style);
        GUI.Label(new Rect(screenPosDet.x - 40, screenPosDet.y - 35, 80, 20), "ChasePlayer", style);
        GUI.Label(new Rect(screenPosAni.x - 40, screenPosAni.y - 55, 100, 20), "ChaseAnimals", style);
        GUI.Label(new Rect(screenPosAtk.x - 40, screenPosAtk.y - 75, 80, 20), "ToOutChase", style);
        GUI.Label(new Rect(screenPosPatrol.x - 40, screenPosPatrol.y - 95, 80, 20), "Patrol", style);

        Handles.EndGUI();
        #endif
    }
}