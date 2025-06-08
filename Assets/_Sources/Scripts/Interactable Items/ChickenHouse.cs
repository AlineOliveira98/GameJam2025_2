using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
public class ChickenHouse : Interactable
{
    [SerializeField] private float delayToBreakEgg;
    [SerializeField] private Chicken egg;
    [SerializeField] private GameObject feather;

    private Collider2D NPCCollider;

    void Start()
    {
        NPCCollider = egg.GetComponent<Collider2D>();
        NPCCollider.enabled = false;
    }

    public override void Interact()
    {
        if (LockInteract) return;

        LockInteract = true;

        base.Interact();

        var player = GameController.Instance.Player;

        feather.transform.position = player.transform.position;

        feather.transform.DOLocalMoveY(1f, 0.5f).OnComplete(() =>
        {
            feather.gameObject.SetActive(false);
            NPCCollider.enabled = true;
            egg.LockedInteraction = true;
            GameController.Instance.HasFeather = true;
        });
        
        
    }
}