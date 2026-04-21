using UnityEngine;
using System.Collections.Generic;
using System;
using Utils;
using UnityEngine.UI;

public class SeedPickupUI : Singleton<SeedPickupUI>
{
    [Serializable]
    private struct SeedPickupPopup
    {
        public float lifeTime;
        public GameObject ui;

        public SeedPickupPopup(float lifetime, GameObject ui)
        {
            this.lifeTime = lifetime;
            this.ui = ui;
        }
    }
    
    [SerializeField] private LinkedList<SeedPickupPopup> seedPickups = new LinkedList<SeedPickupPopup>();
    [SerializeField, Range(1, 10)] private int maxSeedPopups = 3;
    [SerializeField] private GameObject seedPopupPrefab;
    [SerializeField] private VerticalLayoutGroup layoutGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitSingleton();
    }

    // Update is called once per frame
    void Update()
    {
        LinkedListNode<SeedPickupPopup> node = seedPickups.First;
        while (node != null)
        {
            node.Value = new SeedPickupPopup(node.Value.lifeTime - Time.deltaTime, node.Value.ui);
            if(node.Value.lifeTime <= 0)
            {
                Destroy(node.Value.ui);
                seedPickups.Remove();
            }
            node = node.Next;
        }
    }

    public void SeedPickedUp()
    {
        if (seedPickups.Count == maxSeedPopups)
        {
            LinkedListNode<SeedPickupPopup> popup = seedPickups.First;
            Destroy(popup.Value.ui);
            seedPickups.RemoveFirst();
        }



    }
}
