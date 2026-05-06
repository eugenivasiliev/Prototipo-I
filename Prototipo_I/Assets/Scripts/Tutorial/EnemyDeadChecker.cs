using UnityEngine;

public class EnemyDeadChecker : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject seedsMessage;
    [SerializeField] GameObject wall;
    void Update()
    {
        if (enemies[0] == null && enemies[1] == null)
        {
            seedsMessage.SetActive(true);
            Destroy(wall);
            Destroy(gameObject);
        }
    }
}
