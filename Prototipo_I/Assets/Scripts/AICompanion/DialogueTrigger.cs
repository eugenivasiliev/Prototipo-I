using Player;
using System.Collections;
using UnityEngine;

namespace AICompanion
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private int time;
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




        IEnumerator ActivateNextTutorial() {

            yield return new WaitForSeconds(time);

            Destroy(gameObject);

            if (wall != null)
            {
                Destroy(wall);
            }
        }
    }
}