using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float attackRate;

    [Header("References")]
    [SerializeField] private EnemyVisual visual;
    [SerializeField] protected EnemyMovement enemyMovement;

    private float lastAttackTime = -Mathf.Infinity;
    public EnemyVisual Visual { get => visual; }
    public EnemyMovement EnemyMovement { get => enemyMovement; }

    void Start()
    {

    }

    void Update()
    {
        Attack();
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

    private void Attack()
    {
        if (!EnemyMovement.Patrol.IsAttacking || EnemyMovement.Patrol.IsKnockback) return;

        if (EnemyMovement.Patrol.TargetFind == null)
        {
            EnemyMovement.Patrol.StopAttack();
            Debug.Log("Target not find, stoping attack");
            return;
        }

        if (Time.time >= lastAttackTime + attackRate)
        {
            if (EnemyMovement.Patrol.TargetFind.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                Debug.Log("Attack By Cooldown");
                Visual.SetAttack();

                if (damageable.IsDead)
                {
                    EnemyMovement.Patrol.StopAttack();
                }
            }

            lastAttackTime = Time.time;
        }
    }

    public void KnockBack(Vector3 targetPos, float force, float duration)
    {
        EnemyMovement.KnockBack(targetPos, force, duration);
    }
}
