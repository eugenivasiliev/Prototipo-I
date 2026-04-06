using Player;
using UnityEngine;
using System.Collections;

namespace Enemies { 

    public class OctobearAttack : MonoBehaviour
    {
        private float stunTime = 1.0f;
        private float attackCooldown = 1.0f;
        private Collider col;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                col = other;
                StartCoroutine(StunStart());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
                col = null;
        }

        private IEnumerator StunStart() {

            col.GetComponent<PlayerController>().Stun(stunTime);

            yield return new WaitForSeconds(attackCooldown);

            if (col != null)
            {
                StartCoroutine(StunStart());
            }
        }
    }

}