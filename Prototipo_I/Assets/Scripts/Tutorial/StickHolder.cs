using UnityEngine;
using System.Collections;

public class StickHolder : MonoBehaviour
{

    [SerializeField] GameObject[] sticks;
    float time = 0f;
    float Maxtime = 3.0f;

    float speed = 0.5f;

    public IEnumerator Dissolve()
    {


        while (time < Maxtime) { 
        
            time += Time.deltaTime * speed;

            for (int i = 0; i < sticks.Length; i++)
            {
                sticks[i].GetComponent<MaterialInstanceDissolveModifier>().dissolveValue = time;
                Debug.Log("dissolving!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                
            
            }
            yield return null;
        
        }

        Destroy(gameObject);

    }
}
