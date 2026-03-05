using UnityEngine;

public class DBManager : Singleton<DBManager>
{
    [SerializeField] private PlantDB plantDB;
    public PlantDB PlantDB { get => plantDB; }

    [SerializeField] private TowerDB towerDB;
    public TowerDB TowerDB { get => towerDB; }

    private void Start()
    {
        Init();
        plantDB.Init();
        towerDB.Init();
    }

}
