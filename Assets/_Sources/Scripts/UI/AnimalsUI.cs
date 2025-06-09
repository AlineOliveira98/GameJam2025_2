using UnityEngine;
using System.Collections.Generic;

public class AnimalsUI : MonoBehaviour
{
    [System.Serializable]
    public class AnimalSlot
    {
        public AnimalType type;
        public GameObject icon;
        public GameObject savedIcon;
        public GameObject diedIcon;
    }

    [SerializeField] private AnimalSlot[] animalSlotsArray;

    private Dictionary<AnimalType, AnimalSlot> slotMap;

    public static AnimalsUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        slotMap = new Dictionary<AnimalType, AnimalSlot>();
        foreach (var slot in animalSlotsArray)
        {
            if (slotMap.ContainsKey(slot.type))
            {
                Debug.LogError($"Duplicate AnimalType in UI: {slot.type}");
            }
            else
            {
                slotMap.Add(slot.type, slot);
            }
        }
    }


    public void SetSaved(AnimalType type)
    {
        if (!slotMap.ContainsKey(type)) return;

        var slot = slotMap[type];
        slot.icon.SetActive(true);
        slot.savedIcon.SetActive(true);
        slot.diedIcon.SetActive(false);
    }

    public void SetDied(AnimalType type)
    {
        if (!slotMap.ContainsKey(type)) return;

        var slot = slotMap[type];
        slot.icon.SetActive(true);
        slot.savedIcon.SetActive(false);
        slot.diedIcon.SetActive(true);
    }
}
