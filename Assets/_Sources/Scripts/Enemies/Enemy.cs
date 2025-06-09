using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float attackRate;
    [SerializeField] private float attackAnimationDuration;

    [Header("References")]
    [SerializeField] private EnemyVisual visual;
    [SerializeField] protected EnemyMovement enemyMovement;

    private float lastAttackTime = -Mathf.Infinity;
    public EnemyVisual Visual { get => visual; }
    public EnemyMovement EnemyMovement { get => enemyMovement; }
    public float Damage => damage;

    void Start()
    {

    }

    void Update()
    {
        // Attack();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!EnemyMovement.Patrol.Dash.IsDashing) return;

        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
            Debug.Log("Attack By Trigger");
        }
    }

    public bool IsAttackCoroutineRunning { get; set; }

    private async Task Attack()
    {
        if (!EnemyMovement.Patrol.IsAttacking || EnemyMovement.Patrol.IsKnockback) return;
        if (IsAttackCoroutineRunning) return;

        if (EnemyMovement.Patrol.TargetFind == null)
        {
            // EnemyMovement.Patrol.StopAttack();
            return;
        }

        if (Time.time >= lastAttackTime + attackRate)
        {
            IsAttackCoroutineRunning = true;
            Visual.SetAttack();
            await Task.Delay((int)(attackAnimationDuration * 1000));

            if (EnemyMovement.Patrol.TargetFind.TryGetComponent(out IDamageable damageable))
            {
                if (CheckTargetInRange())
                {
                    damageable.TakeDamage(damage);

                    if (damageable.IsDead)
                    {
                        // EnemyMovement.Patrol.StopAttack();
                    }
                }
            }

            lastAttackTime = Time.time;
            
            EnemyMovement.Patrol.IsAttacking = false;
            IsAttackCoroutineRunning = false;
        }
    }

    private bool CheckTargetInRange()
    {
        // Verifica se o alvo ainda está no raio de ataque
        bool stillInRange = false;

        if (EnemyMovement.Patrol.TargetFind != null)
        {
            float dist = Vector2.Distance(
                EnemyMovement.Patrol.TargetFind.position,
                transform.position
            );

            stillInRange = dist <= EnemyMovement.Patrol.RangeAttack; // ajuste para seu campo de range
        }

        return stillInRange;
    }

    public void KnockBack(Vector3 targetPos, float force, float duration)
    {
        EnemyMovement.KnockBack(targetPos, force, duration);
    }
}
