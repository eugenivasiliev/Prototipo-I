using UnityEngine;
using UnityEngine.ProBuilder;

public class ContextKey : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            transform.GetChild(0).gameObject.SetActive(true);

            CompanionDialogueUI.Instance.DisplayTextById("Context Key");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
