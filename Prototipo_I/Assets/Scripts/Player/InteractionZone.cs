using UnityEngine;
using System.Collections.Generic;

public class InteractionZone : MonoBehaviour
{
    [SerializeField] GameObject playerGameObject;

    PlayerController player;

    private SphereCollider sc;

    private List<GameObject> list = new List<GameObject>();

    private IInteractable interactable;

    PlayerController controller;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out interactable))
        {
            //list.Add(other.gameObject);
            //Debug.Log("Entro: " + other.gameObject.name);
            interactable.Bind();
        }
        //Debug.Log("Objetos Interactuables: " + list.Count);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out interactable))
        {
            interactable.Unbind();
        }
        //list.Remove(other.gameObject);
        //Debug.Log("Objetos Interactuables: " + list.Count);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = playerGameObject.GetComponent<PlayerController>();
        sc = GetComponent<SphereCollider>();
        sc.radius = player.InteractionRange;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
