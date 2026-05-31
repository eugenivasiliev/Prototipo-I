using Player;
using System.Collections;
using UnityEngine;

namespace AICompanion
{
    public class DialogueTrigger : MonoBehaviour
    {
        
        private PlayerController playerController;
        [SerializeField] private string dialogueId;
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") { 
                AICompanionDialogueUI.Instance.DisplayTextById(dialogueId);

                Destroy(gameObject);
            }

        }

    }
}