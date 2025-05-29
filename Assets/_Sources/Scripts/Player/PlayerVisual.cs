using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Animator anim;

    void Update()
    {
        anim.SetFloat("Horizontal", player.input.x);
        anim.SetFloat("Vertical", player.input.y);

        anim.SetBool("IsRunning", player.rb.linearVelocity != Vector2.zero);
    }
}
