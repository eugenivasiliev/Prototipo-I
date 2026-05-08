using UnityEngine;

public class PlotInteracted : MonoBehaviour
{
    [SerializeField] GameObject[] beams;
    [SerializeField] GameObject waitDialogue;
    [SerializeField] GameObject console;
    [SerializeField] GameObject dullConsole;

    void Update()
    {
        if (beams[0].activeSelf == false &&
            beams[1].activeSelf == false)
        { 
            waitDialogue.SetActive(true);
            console.SetActive(true);
            dullConsole.SetActive(false);

            Destroy(this);
        }
    }
}
