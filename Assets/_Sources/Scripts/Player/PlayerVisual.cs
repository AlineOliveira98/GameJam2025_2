using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Animator anim;

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
}
