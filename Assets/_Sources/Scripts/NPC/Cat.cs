using UnityEngine;

public class Cat : NPC
{
    public override void Collect()
    {
        if (IsDead || IsSaved || LockedInteraction) return;

        IsSaved = true;
        GameController.Instance.OpenDialogue();
    }

    public void CollectReal()
    {
        AnimalsUI.Instance.SetSaved(animalType);
        GameController.Instance.SaveAnimal(this);
    }
}