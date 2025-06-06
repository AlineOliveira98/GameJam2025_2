using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI victimsCount;

    void OnEnable()
    {
        GameController.OnAnimalSaved += UpdateVictimsCount;
        GameController.OnAnimalDied += UpdateVictimsCount;
    }

    void OnDisable()
    {
        GameController.OnAnimalSaved -= UpdateVictimsCount;
        GameController.OnAnimalDied -= UpdateVictimsCount;
    }

    void Start()
    {
        UpdateVictimsCount();
    }

    private void UpdateVictimsCount(NPC animal = null)
    {
        int amountAnimals = GameController.Instance.AnimalsCurrentNumber;
        string text = amountAnimals > 0 ? $"{amountAnimals}" : $"GO TO THE END";
        victimsCount.text = $"{text}";
    }
}
