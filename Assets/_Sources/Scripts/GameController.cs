using System;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private DialogueSO startDialogue;

    private int totalAnimals;

    public int AnimalsCurrentNumber { get; private set; }
    public int AnimalsDied { get; private set; }
    public int AnimalsSaved { get; private set; }
    public Player Player { get; private set; }

    public static Action OnLivingVictimsChanged;

    void Awake()
    {
        Instance = this;

        Player = FindAnyObjectByType<Player>();
        totalAnimals = FindObjectsByType<NPC>(FindObjectsSortMode.None).Count();

        AnimalsDied = 0;
        AnimalsSaved = 0;
        AnimalsCurrentNumber = totalAnimals;
    }

    void Start()
    {
        if (startDialogue != null)
        {
            DialogueService.StartDialogue(startDialogue);
        }
    }

    public void SaveAnimal()
    {
        AnimalsSaved++;
        AnimalsCurrentNumber--;
        OnLivingVictimsChanged?.Invoke();
    }

    public void KillAnimal()
    {
        AnimalsDied++;
        AnimalsCurrentNumber--;
        OnLivingVictimsChanged?.Invoke();
    }

    public void WateringTree()
    {

    }
}
