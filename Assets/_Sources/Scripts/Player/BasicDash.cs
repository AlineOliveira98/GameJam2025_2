using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;

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

    public bool CanDash => canDash;
    public bool IsDashing => isDashing;

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
        if (!CanDash || direction == Vector2.zero) return;

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

        rb.linearVelocity = direction.normalized * dashSpeed;

        await Task.Delay((int)(dashDuration * 1000));

        rb.linearVelocity = Vector2.zero;

        if (trail != null)
            trail.emitting = false;

        if (agent != null)
        {
            SetDestinationAfterDash(rb.position);

            agent.Warp(rb.position);
            await Task.Yield(); // garante reativa��o segura
            agent.enabled = true;
            await Task.Yield();

            if (pendingDestination.HasValue)
            {
                agent.SetDestination(pendingDestination.Value);
                pendingDestination = null;
            }
        }

        isDashing = false;
        await Task.Delay((int)(dashCooldown * 1000));
        canDash = true;
    }
}
