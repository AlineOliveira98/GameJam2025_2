using System.Threading.Tasks;
using UnityEngine;

public class HeartTree : Interactable
{
    [SerializeField] private float animationDelay;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite emptyTree;

    private bool collected;

    public override void Interact()
    {
        RecoverHealth();
    }

    public async void RecoverHealth()
    {
        if (collected) return;
        collected = true;
        
        anim.SetTrigger("Drop");
        GameController.Instance.Player.Health.RecoverHealth(1);
        await Task.Delay((int)(animationDelay * 1000));

        anim.enabled = false;
        spriteRenderer.sprite = emptyTree;
    }
}
