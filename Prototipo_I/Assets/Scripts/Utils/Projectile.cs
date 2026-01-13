using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 finalPos;
    
    float time = 0.0f;
    public float maxTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        this.transform.position = (time/maxTime) * finalPos + (1- (time / maxTime)) * startPos;

        if (time > maxTime) { 
            Destroy(gameObject);
        }
    }
}
