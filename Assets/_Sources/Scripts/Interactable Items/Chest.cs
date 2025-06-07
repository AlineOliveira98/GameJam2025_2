using DG.Tweening;
using UnityEngine;

public class Chest : Interactable
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject axe;

    public override void Interact()
    {
        base.Interact();

        CanInteract = false;

        GameController.Instance.OpenChest();
        anim.SetTrigger("Open");

        var player = GameController.Instance.Player;

        axe.transform.position = player.transform.position;

        axe.transform.DOLocalMoveY(1f, 0.5f).OnComplete(() =>
        {
            axe.gameObject.SetActive(false);
        });
    }
}
