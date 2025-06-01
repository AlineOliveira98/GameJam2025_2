using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI lineText;
    [SerializeField] private Image characterIcon;
    [SerializeField] private Canvas canvas;

    [Header("Options")]
    [SerializeField] private GameObject optionsContainer;
    [SerializeField] private TextMeshProUGUI[] optionsText;

    [SerializeField] private DialogueSO currentDialogue;
    private int currentLine;

    void OnEnable()
    {
        DialogueService.OnDialogueStarted += StartDialogue;
    }

    void OnDisable()
    {
        DialogueService.OnDialogueStarted -= StartDialogue;
    }

    public void StartDialogue(DialogueSO dialogueSO)
    {
        currentDialogue = dialogueSO;
        currentLine = 0;
        ShowLine();

        canvas.enabled = true;
    }

    private void ShowLine()
    {
        if (currentLine >= currentDialogue.dialogue.Length) return;

        var line = currentDialogue.dialogue[currentLine];

        characterName.text = line.character.characterName;
        characterIcon.sprite = line.character.characterIcon;

        lineText.text = line.dialogue;

        optionsContainer.SetActive(line.options.Length > 0);

        if (line.options.Length > 0)
        {
            ShowOptions();
        }

        void ShowOptions()
        {
            for (int i = 0; i < line.options.Length; i++)
            {
                optionsText[i].text = line.options[i];
                var optionButton = optionsText[i].GetComponentInParent<Button>();
                optionButton.onClick.RemoveAllListeners();
                optionButton.onClick.AddListener(() => SelectOption(i));
            }
        }
    }

    private void SelectOption(int option)
    {
        canvas.enabled = false;
    }

    public void NextLine()
    {
        currentLine++;
        ShowLine();
    }

    public void SkipDialogue()
    {
        canvas.enabled = false;
    }
}
