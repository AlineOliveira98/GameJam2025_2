using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
        }
    }

    private void Attack()
    {
        if (!EnemyMovement.Patrol.IsAttacking) return;

        if (EnemyMovement.Patrol.TargetFind == null)
        {
            EnemyMovement.Patrol.StopAttack();
            return;
        }

        if (Time.time >= lastAttackTime + attackRate)
        {
            if (EnemyMovement.Patrol.TargetFind.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                Visual.SetAttack();

                if (damageable.IsDead)
                {
                    EnemyMovement.Patrol.StopAttack();
                }
            }

            lastAttackTime = Time.time;
        }
    }

}
