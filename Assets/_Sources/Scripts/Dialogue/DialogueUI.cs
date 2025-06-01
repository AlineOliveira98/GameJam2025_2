using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI lineText;
    [SerializeField] private Image characterIcon;

    [Header("Options")]
    [SerializeField] private GameObject optionsContainer;
    [SerializeField] private TextMeshProUGUI[] optionsText;

    private DialogueSO currentDialogue;
    private int currentLine;

    void Start()
    {
        
    }

    public void StartDialogue(DialogueSO dialogueSO)
    {
        currentDialogue = dialogueSO;
        currentLine = 0;
        ShowLine();
    }

    private void ShowLine()
    {
        if (currentLine >= currentDialogue.dialogue.Length - 1) return;

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
            }
        }
    }

    public void NextLine()
    {
        currentLine++;
        ShowLine();
    }

    public void SkipDialogue()
    {
        gameObject.SetActive(false);
    }
}
