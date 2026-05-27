using System;
using System.Collections;
using UnityEngine;

public class FenceDissolver : MonoBehaviour
{
    [SerializeField] GameObject[] things;
    
    [SerializeField] GameObject[] fences;

    [SerializeField] GameObject waitDialogue;
    [SerializeField] GameObject console;
    [SerializeField] GameObject dullConsole;

    void Start()
    {
        
    }

    void Update()
    {
        for (int i = 0; i < things.Length; i++)
        {
            if (things[i] != null) return;
        }



        for (int i = 0; i < fences.Length; i++)
        {
            fences[i].GetComponent<StickHolder>().StartCoroutine(
                fences[i].GetComponent<StickHolder>().Dissolve());

        }


        waitDialogue.SetActive(true);
        if (console != null)
            console.SetActive(true);
        if (dullConsole != null)
            dullConsole.SetActive(false);


        Destroy(gameObject);
    }
}
