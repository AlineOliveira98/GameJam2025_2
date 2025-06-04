using System;
using Unity.Cinemachine;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sprite;

    private Vector2 directionView;
    private Vector2 lastDirection;

    void Update()
    {
        sprite.flipX = directionView.x < transform.position.x;

        if (directionView.x == transform.position.x)
            sprite.flipX = lastDirection.x < transform.position.x;
    }

    public void SetDirection(Vector2 directionView)
    {
        this.directionView = directionView;
        lastDirection = directionView;
    }

    public void SetRunning(bool isRunning)
    {
        anim.SetBool("IsRunning", isRunning);
    }

    public void TriggerDeath()
    {
        anim.SetTrigger("Deadh");
    }

    public void SetAttack()
    {
        anim.SetTrigger("Attack");
    }
}
