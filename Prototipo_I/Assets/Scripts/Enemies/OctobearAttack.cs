using Player;
using UnityEngine;

namespace Enemies { 

    public class OctobearAttack : MonoBehaviour
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