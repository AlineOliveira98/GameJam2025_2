using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;

    [Header("References")]
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private EnemyPatrol patrol;

    private float cooldownTimer;
    private Vector2 dashStartPos;
    public bool IsDashing { get; private set; }
    public EnemyPatrol Patrol { get => patrol; }

    void Start()
    {

    }

    void Update()
    {
        Dashing();
    }

    public void Dash()
    {
        if (IsDashing || cooldownTimer > 0f) return;

        IsDashing = true;
        dashStartPos = rig.position;
        cooldownTimer = dashCooldown;
        Patrol.Agent.isStopped = true;

        var direction = ((Vector2)Patrol.TargetFind.position - rig.position).normalized;

        rig.linearVelocity = direction * dashSpeed;
    }

    private void Dashing()
    {
        if (IsDashing)
        {
            var dashedDistance = (dashStartPos - rig.position).sqrMagnitude;

            if (dashedDistance >= dashDistance * dashDistance)
            {
                IsDashing = false;
                rig.linearVelocity = Vector2.zero;
                Patrol.Agent.isStopped = false;
            }
        }
        else
            cooldownTimer -= Time.deltaTime;
    }
}
