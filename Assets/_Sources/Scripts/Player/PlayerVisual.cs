using TMPro;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetDirection(Vector2 dir)
    {
        anim.SetFloat("Horizontal", dir.x);
        anim.SetFloat("Vertical", dir.y);
    }

    public void SetRunning(bool isRunning)
    {
        anim.SetBool("IsRunning", isRunning);
    }

    public void SetWatering()
    {
        anim.SetTrigger("IsWatering");
    }

    public void SetAttack()
    {
        anim.SetTrigger("Attack");
    }

    public void SetDeath()
    {
        anim.SetTrigger("Death");
    }

    public void SetInvisible(bool enable)
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, enable ? 0.5f : 1f);
    }
}
