using UnityEngine;

public class EnemyDeadChecker : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject seedsMessage;
    [SerializeField] GameObject wall;
    void Update()
    {

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                return;
            }
        }

        if (seedsMessage != null) 
            seedsMessage.SetActive(true);

        if (wall != null) 
            Destroy(wall);

        Destroy(gameObject);
    }
}
