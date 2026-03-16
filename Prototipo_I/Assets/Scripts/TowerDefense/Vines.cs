using Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Vines : MonoBehaviour
    {
        [SerializeField] private GameObject particle;

        
        private Dictionary<Collider, GameObject> allParticles = new Dictionary<Collider, GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<EnemyAI>() != null)
            {
                other.GetComponent<EnemyAI>().SlowDown();

                GameObject p = Instantiate(particle, other.transform.position, Quaternion.identity, other.transform);
                
                allParticles.Add(other, p);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<EnemyAI>() != null)
            {
                GameObject p = allParticles[other];

                Destroy(p);

                allParticles.Remove(other);
                other.GetComponent<EnemyAI>().UnSlowDown();                

            }
        }
    }
}