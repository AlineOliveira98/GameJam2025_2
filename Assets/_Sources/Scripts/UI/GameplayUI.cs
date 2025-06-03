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
        victimsCount.text = GameController.Instance.AnimalsCurrentNumber.ToString();
    }
}
