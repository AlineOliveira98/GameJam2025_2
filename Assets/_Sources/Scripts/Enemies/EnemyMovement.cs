using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashDuration;

    [Header("References")]
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private EnemyVisual visual;

    private float dashTimer;
    private float cooldownTimer;
    public bool IsDashing { get; private set; }

    void Start()
    {

    }

    void Update()
    {
        Dashing();
    }

    public void Dash(Vector2 direction)
    {
        if (IsDashing || cooldownTimer > 0f) return;

        IsDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;

        rig.linearVelocity = direction.normalized * dashSpeed;

        if (visual != null) visual.SetDashing(true);
    }

    private void Dashing()
    {
        if (IsDashing)
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                IsDashing = false;
                rig.linearVelocity = Vector2.zero;

                if (visual != null) visual.SetDashing(false);
            }
        }
        else
            cooldownTimer -= Time.deltaTime;
    }
}
