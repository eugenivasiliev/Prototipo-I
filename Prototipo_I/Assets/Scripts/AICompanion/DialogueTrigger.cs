using UnityEngine;

namespace AICompanion
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private string dialogueId;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
                AICompanionDialogueUI.Instance.DisplayTextById(dialogueId);
        }
    }
}