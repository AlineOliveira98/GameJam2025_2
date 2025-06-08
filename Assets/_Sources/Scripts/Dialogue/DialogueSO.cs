using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Scriptable Objects/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public DialogueItem[] dialogue;

    public Action<int> OnOptionSelected;
}

[Serializable]
public class DialogueItem
{
    public CharacterSO character;
    [TextArea] public string dialogue;
    public string[] options;
}
