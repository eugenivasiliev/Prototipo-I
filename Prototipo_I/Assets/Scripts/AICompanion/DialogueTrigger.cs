using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string dialogueId;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            CompanionDialogueUI.Instance.DisplayTextById(dialogueId);
    }
}
