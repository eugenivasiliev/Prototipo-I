using Player;
using System.Collections;
using UnityEngine;

namespace AICompanion
{
    public class DialogueTrigger : MonoBehaviour
    {
        private float time = 5.0f;
        private PlayerController playerController;
        [SerializeField] private string dialogueId;
        [SerializeField] private bool shouldStop;
        [SerializeField] private GameObject wall;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") { 
                AICompanionDialogueUI.Instance.DisplayTextById(dialogueId);

                if (shouldStop) 
                    StartCoroutine(StopForATime(other));
            }

        }



        IEnumerator StopForATime(Collider c)
        {
            c.GetComponent<PlayerController>().MovementLocked = true;

            yield return new WaitForSeconds(time);
            
            c.GetComponent<PlayerController>().MovementLocked = false;

            ActivateNextTutorial();
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