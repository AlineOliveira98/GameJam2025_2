using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Scriptable Objects/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public DialogueItem[] dialogue;
}

[Serializable]
public class DialogueItem
{
    public CharacterSO character;
    [TextArea] public string dialogue;
}
