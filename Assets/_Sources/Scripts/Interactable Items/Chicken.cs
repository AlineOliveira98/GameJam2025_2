using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Chicken : NPC
{
    [SerializeField] private float delayAnimation;
    public async void OnInteract()
    {
        if (!GameController.Instance.HasFeather)
        {
            Debug.Log("You need a Feather");
            return;
        }

        Animator.SetTrigger("Break");

        await UniTask.Delay(TimeSpan.FromSeconds(delayAnimation));
        LockedInteraction = false;
    }
}