using Player;
using Enemies;
using UnityEngine;


namespace Enemies
{ 
    public class Octobear : MonoBehaviour
    {

        private float stunTime = 1.0f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<PlayerController>().Stun(stunTime);
                
            }
        }
    }

}