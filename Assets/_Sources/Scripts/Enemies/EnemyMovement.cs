using System;
using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private EnemyPatrol patrol;

    public EnemyPatrol Patrol { get => patrol; }
    public Rigidbody2D Rig => rig;

    void Start()
    {

    }

    public async void KnockBack(Vector3 targetPos, float force, float duration)
    {
        Patrol.IsKnockback = true;
        var direction = (transform.position - targetPos).normalized;
        rig.linearVelocity = direction.normalized * force;
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        rig.linearVelocity = Vector2.zero;
        Patrol.IsKnockback = false;
    }
}
