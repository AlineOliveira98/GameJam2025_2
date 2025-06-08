using TMPro;
using UnityEngine;

public class ItemInventoryUI : MonoBehaviour
{
    public string idItem;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject enabledItem;
    [SerializeField] private GameObject disabledItem;
}
