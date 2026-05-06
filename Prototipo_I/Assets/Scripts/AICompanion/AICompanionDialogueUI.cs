using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Utils;

namespace AICompanion
{
    public class AICompanionDialogueUI : Singleton<AICompanionDialogueUI>
    {
        [SerializeField] private RectTransform dialogueUI;
        [SerializeField] private TMP_Text dialogueText;

        [SerializeField] private Tween<Vector2> textPosTween;

        [SerializeField] private DialogueDB dialogueDB;

        private void Start()
        {
            InitSingleton();
            dialogueDB.Init();
            textPosTween.Reset();
            dialogueUI.anchoredPosition = textPosTween.value;
        }

        private void Update()
        {
            if (TweenUtil.Update(Time.deltaTime, ref textPosTween))
                dialogueUI.anchoredPosition = textPosTween.value;
        }

        public void DisplayText(ref OneTimeDialogue dialogue)
        {
            if (dialogue.hasTriggered) return;

            textPosTween.Reset();
            StopAllCoroutines();

            //dialogueText.text = dialogue.Text;
            dialogue.hasTriggered = true;
            textPosTween.SetActive(true);

            StartCoroutine(HideTextIn(dialogue.lifeTime));
        }

        public bool DisplayTextById(string id)
        {

            string localizedString = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizarionTableCollection", id);


            dialogueText.text = localizedString;

            if (!dialogueDB.ContainsKey(id)) return false;

            OneTimeDialogue dialogue = dialogueDB[id];
            DisplayText(ref dialogue);
            dialogueDB[id] = dialogue;
            return true;
        }

        private IEnumerator HideTextIn(float time)
        {
            yield return new WaitForSeconds(time);
            textPosTween.Reverse();
            textPosTween.SetActive(true);
        }
    }
}