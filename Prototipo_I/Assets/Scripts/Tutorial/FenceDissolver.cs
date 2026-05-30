using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceDissolver : MonoBehaviour
{
    [SerializeField] private List<GameObject> conditions;
    
    [SerializeField] private GameObject[] fences;

    [SerializeField] private GameObject waitDialogue;
    [SerializeField] private GameObject console;
    [SerializeField] private GameObject dullConsole;

    void Update()
    {
        for (int i = 0; i < conditions.Count; i++)
            if (conditions[i] == null || !conditions[i].activeSelf) conditions.RemoveAt(i);

        if (conditions.Count > 0) return;

        for (int i = 0; i < fences.Length; i++)
            fences[i].GetComponent<DissolvingObject>().Dissolve();

        if(waitDialogue != null)
            waitDialogue.SetActive(true);
        if (console != null)
            console.SetActive(true);
        if (dullConsole != null)
            dullConsole.SetActive(false);

        this.enabled = false;
    }
}
