using System.Collections;
using UnityEngine;

public class AfterWave : MonoBehaviour
{

    [SerializeField] GameObject[] messages;
    [SerializeField] GameObject[] plots;
    [SerializeField] GameObject[] signals;

    [SerializeField] float waiting;


    private int i = 0;

    public void Executed() {

        StartCoroutine(Go());
    
    }

    public IEnumerator Go() { 
        
        yield return new WaitForSeconds(waiting);

        if (i < messages.Length) { 
            if (messages[i] != null)
                messages[i].SetActive(true);
            if (plots[i] != null) 
                plots[i].SetActive(true);
            
            i++;            
        }
    }
}
