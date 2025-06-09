using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI lineText;
    [SerializeField] private Image characterIcon;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button closeButton;

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

        nextButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(false);

        UIController.Instance.OpenPanel(PanelType.Dialogue);
    }

    private void ShowLine()
    {
        if (currentLine >= currentDialogue.dialogue.Length) return;

        var line = currentDialogue.dialogue[currentLine];

        characterName.text = line.character.characterName;
        characterIcon.sprite = line.character.characterIcon;

        lineText.text = line.dialogue;

        optionsContainer.SetActive(line.options.Length > 0);
        
        if (currentLine >= currentDialogue.dialogue.Length - 1)
        {
            nextButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);
        }

        if (line.options.Length > 0)
        {
            ShowOptions();
            closeButton.gameObject.SetActive(false);
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
        currentDialogue.OnOptionSelected?.Invoke(option);
        CloseDialogue();
    }

    public void NextLine()
    {
        currentLine++;
        ShowLine();
    }

    public void CloseDialogue()
    {
        GameController.Instance.PauseGame(false);
        FindAnyObjectByType<Cat>().CollectReal();
        UIController.Instance.OpenPanel(PanelType.Gameplay);
        DialogueService.FinishDialogue();
    }
}
