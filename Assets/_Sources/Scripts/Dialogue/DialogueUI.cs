using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI lineText;
    [SerializeField] private Image characterIcon;

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

        characterName.text = currentDialogue.dialogue[currentLine].character.characterName;
        characterIcon.sprite = currentDialogue.dialogue[currentLine].character.characterIcon;

        lineText.text = currentDialogue.dialogue[currentLine].dialogue;
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
