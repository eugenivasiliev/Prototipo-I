using UnityEngine;

public class TowerDatabase : MonoBehaviour
{
    private static TowerDatabase instance;
    public static TowerDatabase Instance {  get { return instance; } }

    [SerializeField] private TowerData[] towerDatas = new TowerData[0];
    public TowerData[] PlantDatas { get { return towerDatas; } }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public TowerData GetTowerByName(string name)
    {
        foreach (var towerData in towerDatas)
            if(towerData.name == name) return towerData;

        return null;
    }
}
