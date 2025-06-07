using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
        base.Interact();
        
        RecoverHealth();
    }

    public async void RecoverHealth()
    {
        anim.SetTrigger("Drop");
        GameController.Instance.Player.Health.RecoverHealth(1);
        await UniTask.Delay((int)(animationDelay * 1000));

        anim.enabled = false;
        spriteRenderer.sprite = emptyTree;
        
        LockInteract = false;
    }
}
