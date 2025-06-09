using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HeartTree : Interactable
{
    [SerializeField] private float animationDelay;
    [SerializeField] private float cooldown = 10f;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite emptyTree;
    
    private bool onCooldown = false;
    private Sprite originalSprite;

    void Start()
    {
        originalSprite = spriteRenderer.sprite;
    }

    public override void Interact()
    {
        base.Interact();

        if (!onCooldown)
            RecoverHealth();
    }

    public async void RecoverHealth()
    {
        if (LockInteract) return;

        LockInteract = true;
        onCooldown = true;
        anim.SetBool("IsEmpty", true);
        GameController.Instance.Player.Health.RecoverHealth(1);
        await UniTask.Delay((int)(animationDelay * 1000));

        anim.enabled = false;
        spriteRenderer.sprite = emptyTree;
        
        await UniTask.Delay((int)(cooldown * 1000));
        anim.enabled = true;
        LockInteract = false;
        // spriteRenderer.sprite = originalSprite;
        anim.SetBool("IsEmpty", false);
        onCooldown = false;
    }
}
