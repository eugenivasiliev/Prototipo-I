using Enemies;
using UnityEngine;

namespace TowerDefense
{
    public class Vines : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<EnemyAI>() != null)
            {
                other.GetComponent<EnemyAI>().SlowDown();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<EnemyAI>() != null)
            {
                other.GetComponent<EnemyAI>().UnSlowDown();
            }
        }
    }
}