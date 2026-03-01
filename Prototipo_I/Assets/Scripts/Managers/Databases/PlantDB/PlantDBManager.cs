using UnityEngine;

public class PlantDBManager : Singleton<PlantDBManager>
{ 

    [SerializeField] private PlantDB dB;
    public PlantDB DB { get => dB; }

    private void Start()
    {
        Init();
        dB.Init();
    }
}
