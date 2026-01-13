using System.Collections.Generic;
using UnityEngine;

public class AICompanionWavePrediction : MonoBehaviour
{
    WaveDB wM;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenMenu()
    {
        //TODO: Activate all elements
        List<string> nextWave = wM.nextWave;
    }
}
