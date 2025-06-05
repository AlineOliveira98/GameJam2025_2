using System;
using UnityEngine;

public class WaterWell : Interactable
{
    [SerializeField] private WaterWell waterWellRef;
    public Transform spawnArea;

    public static Action<WaterWell> OnTeleportActivated;

    public override void Interact()
    {
        base.Interact();
        OnTeleportActivated.Invoke(waterWellRef);
    }
}
