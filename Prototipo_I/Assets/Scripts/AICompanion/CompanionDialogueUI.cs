using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public struct OneTimeDialogue
{
    [SerializeField] private string id;
    public string Id { get { return id; } }
    [SerializeField] private string text;
    public string Text { get { return text; } }
    public bool hasTriggered;
}

public class CompanionDialogueUI : Singleton<CompanionDialogueUI>
{
    [SerializeField, Range(0, 10)] private float textShowTime;

    [SerializeField] private RectTransform dialogueUI;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private Tween<Vector2> textPosTween;

    [SerializeField] private List<OneTimeDialogue> dialogueList;
    public Dictionary<string, OneTimeDialogue> DialogueList { get; private set; }

    private void Start()
    {
        InitSingleton();
        textPosTween.Reset();
        dialogueUI.anchoredPosition = textPosTween.value;

        DialogueList = new Dictionary<string, OneTimeDialogue>();
        foreach (var dialogue in dialogueList) 
            DialogueList.Add(dialogue.Id, dialogue);
    }

    private void Update()
    {
        if(TweenUtil.Update(Time.deltaTime, ref textPosTween)) 
            dialogueUI.anchoredPosition = textPosTween.value;
    }

    public void DisplayText(ref OneTimeDialogue dialogue)
    {
        if (dialogue.hasTriggered) return;

        dialogueText.text = dialogue.Text;
        dialogue.hasTriggered = true;
        textPosTween.SetActive(true);

        StartCoroutine(HideTextIn(textShowTime));
    }

    public bool DisplayTextById(string id)
    {
        if(!DialogueList.ContainsKey(id)) return false;

        OneTimeDialogue dialogue = DialogueList[id];
        DisplayText(ref dialogue);
        DialogueList[id] = dialogue;
        return true;
    }

    private IEnumerator HideTextIn(float time)
    {
        yield return new WaitForSeconds(time);
        textPosTween.Reverse();
        textPosTween.SetActive(true);
    }
}
