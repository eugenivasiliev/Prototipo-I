using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueDB", menuName = "Scriptable Objects/Databases/DialogueDB")]
public class DialogueDB : ScriptableObject
{
    [SerializeField] private List<OneTimeDialogue> dialogueList;
    public Dictionary<string, OneTimeDialogue> DialogueList { get; private set; }

    public void Init()
    {
        DialogueList = new Dictionary<string, OneTimeDialogue>();
        foreach (var dialogue in dialogueList)
            DialogueList.Add(dialogue.Id, dialogue);
    }

    public bool ContainsKey(string id) => DialogueList.ContainsKey(id);
    public OneTimeDialogue this[string id] {
        get => DialogueList [id];
        set => DialogueList [id] = value;
    }
}
