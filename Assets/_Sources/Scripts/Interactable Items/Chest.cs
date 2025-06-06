using UnityEngine;

public class Chest : Interactable
{
    [SerializeField] private Animator anim;
    public override void Interact()
    {
        base.Interact();

        GameController.Instance.OpenChest();
        anim.SetTrigger("Open");
    }
}
