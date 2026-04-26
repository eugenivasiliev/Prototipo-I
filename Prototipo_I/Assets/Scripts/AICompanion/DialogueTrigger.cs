using Player;
using System.Collections;
using UnityEngine;

namespace AICompanion
{
    public class DialogueTrigger : MonoBehaviour
    {
        
        private PlayerController playerController;
        [SerializeField] private string dialogueId;
        
        [SerializeField] private GameObject wall;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") { 
                AICompanionDialogueUI.Instance.DisplayTextById(dialogueId);

                ActivateNextTutorial();
            }

        }




        void ActivateNextTutorial() {
            Destroy(gameObject);

            if (wall != null)
            {
                Destroy(wall);
            }
        }
    }
}