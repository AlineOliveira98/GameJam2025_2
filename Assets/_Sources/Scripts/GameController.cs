using System;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private int animalsAmoutDiedToDefeat = 5;
    [SerializeField] private int animalsAmoutSavedToVictory = 7;

    [Header("Audio")]
    [SerializeField] private AudioClip[] gameplayMusics;
    [SerializeField] private AudioClip animalSavedAudio;

    [SerializeField] private SkillType skillTypeWhenSavedAnimals;

    [SerializeField] private GameObject winDialogueObject;

    private int totalAnimals;

    public static bool GameStarted { get; private set; }
    public static bool GameIsOver { get; private set; }

    public int AnimalsCurrentNumber { get; private set; }
    public int AnimalsDied { get; private set; }
    public int AnimalsSaved { get; private set; }
    public Player Player { get; private set; }
    public bool HasAxe { get; private set; }
    public bool HasFeather { get; set; }

    public static Action<NPC> OnAnimalSaved;
    public static Action<NPC> OnAnimalDied;
    public static Action OnSavedAnimalAmountReached;
    public static Action OnDeadAnimalLimitReached;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Player = FindAnyObjectByType<Player>();
        totalAnimals = FindObjectsByType<NPC>(FindObjectsSortMode.None).Count();

        AnimalsDied = 0;
        AnimalsSaved = 0;
        AnimalsCurrentNumber = totalAnimals;

        GameStarted = false;
        GameIsOver = false;
    }

    public void SaveAnimal(NPC animal)
    {
        AnimalsSaved++;
        AnimalsCurrentNumber--;
        OnAnimalSaved?.Invoke(animal);

        if (AnimalsSaved >= animalsAmoutSavedToVictory)
        {
            GameOver();
            OnSavedAnimalAmountReached?.Invoke();
        }

        if (!SkillController.Instance.HasSkill(skillTypeWhenSavedAnimals))
        {
            SkillController.Instance.CollectSkill(skillTypeWhenSavedAnimals);
        }

        AudioController.PlaySFX(animalSavedAudio);
    }

    public void KillAnimal(NPC animal)
    {
        AnimalsDied++;
        AnimalsCurrentNumber--;
        OnAnimalDied?.Invoke(animal);

        if (AnimalsDied >= animalsAmoutDiedToDefeat)
        {
            GameOver();
            OnDeadAnimalLimitReached?.Invoke();
        }
    }

    public void StartGameplay()
    {
        GameStarted = true;
        var randomValue = UnityEngine.Random.Range(0, gameplayMusics.Length);
        AudioController.Instance.PlayMusic(gameplayMusics[randomValue]);
        CameraController.Instance.SetCamera(CameraType.Gameplay);
    }

    public void GameOver()
    {
        GameIsOver = true;
    }

    public void OpenChest()
    {
        HasAxe = true;
        Debug.Log("Chest Opened");
    }
}
