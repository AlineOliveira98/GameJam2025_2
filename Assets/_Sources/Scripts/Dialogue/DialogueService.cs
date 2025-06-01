using System;
using UnityEngine;

public class DialogueService
{
    public static Action<DialogueSO> OnDialogueStarted;

    public static void StartDialogue(DialogueSO dialogue)
    {
        OnDialogueStarted?.Invoke(dialogue);
    }
}
