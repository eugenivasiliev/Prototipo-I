using UnityEngine;

public class NeedyPlot : MonoBehaviour
{

    private bool touching;

    void Start()
    {
        
    }

    void Update()
    {
        if (touching)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                this.gameObject.SetActive(false);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {        
        if (other.tag == "Player") 
            touching = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            touching = true;
    }

}
