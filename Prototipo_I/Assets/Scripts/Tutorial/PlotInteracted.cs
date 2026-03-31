using UnityEngine;

public class PlotInteracted : MonoBehaviour
{
    [SerializeField] GameObject[] plots;
    [SerializeField] GameObject waitDialogue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (plots[0].activeSelf == false &&
            plots[1].activeSelf == false &&
            plots[2].activeSelf == false &&
            plots[3].activeSelf == false) 
        { 
            waitDialogue.SetActive(true);
        }
    }
}
