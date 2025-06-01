using System;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private DialogueSO startDialogue;

    private int victimsTotalNumber;
    private int deadVictims;
    private int victimsSaved;

    public int VictimsCurrentNumber { get; private set; }

    public static Action OnLivingVictimsChanged;

    void Awake()
    {
        Instance = this;

        victimsTotalNumber = FindObjectsByType<NPC>(FindObjectsSortMode.None).Count();

        deadVictims = 0;
        victimsSaved = 0;
        VictimsCurrentNumber = victimsTotalNumber;
    }

    void Start()
    {
        if (startDialogue != null)
        {
            DialogueService.StartDialogue(startDialogue);
        }
    }

    public void SaveVictim()
    {
        victimsSaved++;
        VictimsCurrentNumber--;
        OnLivingVictimsChanged?.Invoke();
    }

    public void KillVictim()
    {
        deadVictims++;
        VictimsCurrentNumber--;
        OnLivingVictimsChanged?.Invoke();
    }
}
