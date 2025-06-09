using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class BasicDash : IDash
{
    private readonly Rigidbody2D rb;
    private readonly NavMeshAgent agent;
    private readonly TrailRenderer trail;
    private readonly float dashSpeed;
    private readonly float dashDuration;
    private readonly float dashCooldown;

    private bool isDashing = false;
    private bool canDash = true;
    private Vector3? pendingDestination = null;

    public bool CanDash { get { return canDash; } set { canDash = value; } }
    public bool IsDashing => isDashing;
    public bool LockDash;

    public BasicDash(Rigidbody2D rb, float dashSpeed, float dashDuration, float dashCooldown, NavMeshAgent agent = null, TrailRenderer trail = null)
    {
        this.rb = rb ?? throw new System.ArgumentNullException(nameof(rb));
        this.agent = agent;
        this.trail = trail;
        this.dashSpeed = dashSpeed;
        this.dashDuration = dashDuration;
        this.dashCooldown = dashCooldown;
    }

    public void SetDestinationAfterDash(Vector3 position)
    {
        pendingDestination = position;
    }

    public async void TryDash(Vector2 direction)
    {
        isDashing = true;
        canDash = false;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.enabled = false;
        }

        if (trail != null)
        {
            trail.Clear();             // limpa qualquer sobra anterior
            trail.emitting = true;     // ativa o rastro
        }

        float maxDashDistance = dashSpeed * dashDuration;
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, maxDashDistance, LayerMask.GetMask("Wall"));
        float dashDistance = hit.collider != null ? hit.distance : maxDashDistance;
        Vector2 dashTarget = rb.position + direction.normalized * dashDistance;

        float elapsed = 0f;
        Vector2 startPos = rb.position;
        while (elapsed < dashDuration)
        {
            if(!LockDash) rb.MovePosition(Vector2.Lerp(startPos, dashTarget, elapsed / dashDuration));
            elapsed += Time.fixedDeltaTime;
            await Task.Yield();
        }
        rb.position = dashTarget;
        rb.linearVelocity = Vector2.zero;

        if (trail != null)
            trail.emitting = false;

        if (agent != null)
        {
            SetDestinationAfterDash(rb.position);

            agent.Warp(rb.position);
            agent.enabled = true;

            if (pendingDestination.HasValue)
            {
                agent.SetDestination(pendingDestination.Value);
                pendingDestination = null;
            }
        }

        isDashing = false;
        await UniTask.Delay((int)(dashCooldown * 1000));
        canDash = true;
    }

    public async void CooldownDash()
    {
        canDash = false;
        await UniTask.Delay((int)(dashCooldown * 1000));
        canDash = true;
    }
}
