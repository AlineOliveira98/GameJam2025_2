using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TreeWithCat : Interactable
{
    [SerializeField] private float delayToCutTree;
    [SerializeField] private float delayCatAnimation;
    [SerializeField] private Animator anim;
    [SerializeField] private Animator cat;
    [SerializeField] private Ease easeCatFall;

    private Collider2D catCollider;

    void Start()
    {
        catCollider = cat.GetComponent<Collider2D>();
        catCollider.enabled = false;
    }

    public override async void Interact()
    {
        base.Interact();

        if (!GameController.Instance.HasAxe)
        {
            Debug.Log("You need a Axe");
            return;
        }

        anim.SetTrigger("Cut");

        await UniTask.Delay(TimeSpan.FromSeconds(delayToCutTree));

        cat.transform.DOLocalMoveY(transform.position.y - 2f, 0.5f).SetEase(easeCatFall).OnComplete(() =>
        {
            cat.SetTrigger("InsideBox");
        });

        await UniTask.Delay(TimeSpan.FromSeconds(delayCatAnimation));
        
        catCollider.enabled = true;
    }
}
