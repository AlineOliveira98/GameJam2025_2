using System;
using UnityEngine;

public class DialogueService
{
    public static Action<DialogueSO> OnDialogueStarted;
    public static Action OnDialogueFinished;

    public static void StartDialogue(DialogueSO dialogue)
    {
        OnDialogueStarted?.Invoke(dialogue);
    }

    public static void FinishDialogue()
    {
        OnDialogueFinished?.Invoke();
    }
}
