using TMPro;
using UnityEngine;

public class AnimalsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI victimsCount;

    void OnEnable()
    {
        GameController.OnLivingVictimsChanged += UpdateVictimsCount;
    }

    void OnDisable()
    {
        GameController.OnLivingVictimsChanged -= UpdateVictimsCount;
    }

    void Start()
    {
        UpdateVictimsCount();
    }

    private void UpdateVictimsCount()
    {
        victimsCount.text = GameController.Instance.VictimsCurrentNumber.ToString();
    }
}
