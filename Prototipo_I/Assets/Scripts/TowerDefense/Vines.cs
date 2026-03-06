using UnityEngine;

public class Vines : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyAI>() != null) {
            Debug.Log("Slowing down");
            other.GetComponent<EnemyAI>().Slow();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EnemyAI>() != null) {
            other.GetComponent<EnemyAI>().UnSlow();
        }
    }
}
