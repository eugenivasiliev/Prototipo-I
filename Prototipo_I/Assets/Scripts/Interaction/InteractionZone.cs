using UnityEngine;
using System.Collections.Generic;

public class InteractionZone : MonoBehaviour
{
    [SerializeField] GameObject playerGameObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
            interactable.Bind();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IInteractable interactable))
            interactable.Unbind();
    }

    void Start()
    {
        this.GetComponent<SphereCollider>().radius = playerGameObject.GetComponent<PlayerController>().InteractionRange;
    }
}
