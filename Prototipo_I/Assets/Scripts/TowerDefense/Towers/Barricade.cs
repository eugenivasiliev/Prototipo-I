using Enemies;
using UnityEngine;
using static UnityEngine.ParticleSystem;


namespace TowerDefense{

    public class Barricade : MonoBehaviour
    {

        void Start()
        {

        }


        void Update()
        {

        }



        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<EnemyAI>() != null)
            {
                other.GetComponent<EnemyAI>().GetBarricade(transform);

            }
        }

        private void OnDestroy()
        {
            
        }

    }


}

