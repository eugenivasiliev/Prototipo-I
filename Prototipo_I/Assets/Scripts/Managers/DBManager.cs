using Farm;
using TowerDefense;
using UnityEngine;

namespace Utils
{
    public class DBManager : Singleton<DBManager>
    {
        [SerializeField] private PlantDB plantDB;
        public PlantDB PlantDB { get => plantDB; }

        [SerializeField] private TowerDB towerDB;
        public TowerDB TowerDB { get => towerDB; }
        [SerializeField] private TowerDecalDB towerDecalDB;
        public TowerDecalDB TowerDecalDB { get => towerDecalDB; }

        private void Start()
        {
            InitSingleton();
            plantDB.Init();
            towerDB.Init();
            towerDecalDB.Init();
        }

    }
}