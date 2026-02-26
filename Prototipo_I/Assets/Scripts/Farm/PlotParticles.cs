using System.Collections;
using UnityEngine;

public class PlotParticles : MonoBehaviour
{
    [SerializeField] private float time;
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct() { 
        
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
