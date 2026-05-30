using UnityEngine;

public class PlotInteracted : MonoBehaviour
{
    [SerializeField] GameObject[] beams;
    [SerializeField] GameObject waitDialogue;
    [SerializeField] GameObject console;
    [SerializeField] GameObject dullConsole;

    void Update()
    {
        for (int i = 0; i < beams.Length; i++) {
            if (beams[i].activeSelf == true) return;
        }

            waitDialogue.SetActive(true);
        if (console != null)
            console.SetActive(true);
        if (dullConsole != null) 
            dullConsole.SetActive(true);

        Destroy(this.gameObject);
    }
}
