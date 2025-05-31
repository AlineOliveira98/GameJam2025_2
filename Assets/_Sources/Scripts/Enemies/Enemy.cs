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
    [SerializeField] private EnemyPatrol patrol;

    private float lastAttackTime = -Mathf.Infinity;

    void Start()
    {

    }

    void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (!patrol.IsAttacking) return;

        if (patrol.TargetFind == null)
        {
            patrol.StopAttack();
            return;
        }

        if (Time.time >= lastAttackTime + attackRate)
            {
                if (patrol.TargetFind.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damage);

                    if (damageable.IsDead)
                    {
                        patrol.StopAttack();
                    }
                }

                lastAttackTime = Time.time;
            }
    }

}
