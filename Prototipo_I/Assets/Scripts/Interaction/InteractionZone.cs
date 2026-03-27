using UnityEngine;
using System.Collections.Generic;
using Player;

namespace Utils
{
    public class InteractionZone : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
                interactable.Bind();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
                interactable.Unbind();
        }

        void Start()
        {
            this.GetComponent<SphereCollider>().radius = playerController.InteractionRange;
        }
    }
}