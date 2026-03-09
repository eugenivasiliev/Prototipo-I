using UnityEngine;
using UnityEngine.ProBuilder;

public class Bomb : MonoBehaviour
{
    public Vector3 startPos;
    public Transform finalPos;

    float time = 0.0f;
    public float maxTime;
    public float maxHeight;
    private float height = 4;

    public GameObject explotion;
    void Start()
    {
        
    }

    
    void Update()
    {
        time += Time.deltaTime;
        if (finalPos != null)
        {

            float t = time / maxTime; 
            float curveCurrentHeight = height * maxHeight * t * (1 - t);

            this.transform.position = t * finalPos.position + (1 - t) * startPos;
            this.transform.position = new Vector3(
                t * finalPos.position.x + (1 - t) * startPos.x,
                Mathf.Lerp(startPos.y, finalPos.position.y, t) + curveCurrentHeight,
                t * finalPos.position.z + (1 - t) * startPos.z
                );



        }


        if (time > maxTime)
        {
            Instantiate(explotion, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
        }
    }
}
