using System;
using UnityEngine;

public class YggDrasil : Interactable
{
    [SerializeField] private TreeStage[] stages;

    private int currentWater;
    private int currentStage;

    private int waterCollected => GameController.Instance.AnimalsSaved;

    public static Action OnTreeWatered;

    void Start()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].visual.SetActive(false);
        }

        stages[0].visual.SetActive(true);

        currentStage = 0;
        currentWater = 0;
    }

    public override void Interact()
    {
        Watering();
    }

    private void Watering()
    {
        if (currentWater >= waterCollected)
        {
            Debug.Log("No Water enught");
            return;
        }

        currentWater++;
        OnTreeWatered.Invoke();

        Grow();
    }

    private void Grow()
    {
        if (currentStage >= stages.Length - 1) return;

        Debug.Log($"{stages[currentStage + 1].amountToGrow} - {currentWater}");
        if (stages[currentStage + 1].amountToGrow > currentWater) return;

        stages[currentStage].visual.SetActive(false);
        stages[currentStage + 1].visual.SetActive(true);
        currentStage++;
    }
}

[Serializable]
public class TreeStage
{
    public int amountToGrow;
    public GameObject visual;
}
