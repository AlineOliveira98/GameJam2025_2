using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public ItemInventoryUI axeSlot;
    public ItemInventoryUI featherSlot;
    public ItemInventoryUI blueFlowerSlot;
    public ItemInventoryUI pinkFlowerSlot;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void GetAxe()
    {
        axeSlot.Enable(true);
    }

    public void GetFeather()
    {
        featherSlot.Enable(true);
    }

    public void GetBlueFlower(int amount)
    {
        blueFlowerSlot.UpdateAmount(amount);
        
        if (amount >= 3)
            blueFlowerSlot.Enable(true);
    }

    public void GetPinkFlower()
    {
        pinkFlowerSlot.Enable(true);
    }
}
