using TMPro;
using UnityEngine;

public class ItemInventoryUI : MonoBehaviour
{
    public string idItem;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject enabledItem;
    [SerializeField] private GameObject disabledItem;

    public void Enable(bool enable)
    {
        enabledItem.SetActive(enable);
        disabledItem.SetActive(!enable);
    }

    public void UpdateAmount(int amount)
    {
        amountText.text = $"{amount}/3";
    }
}
