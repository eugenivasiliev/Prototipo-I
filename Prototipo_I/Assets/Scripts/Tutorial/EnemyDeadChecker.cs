using UnityEngine;

public class EnemyDeadChecker : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject seedsMessage;
    [SerializeField] GameObject wall;


    private float waitTime = 0f;
    [SerializeField] private float waitUntilActivation = 0f;


    void Update()
    {

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                return;
            }
        }

        waitTime += Time.deltaTime;

        if (waitTime > waitUntilActivation)
        {
            if (seedsMessage != null) 
                seedsMessage.SetActive(true);

            if (wall != null) 
                Destroy(wall);

            Destroy(gameObject);
        }
    }
}
