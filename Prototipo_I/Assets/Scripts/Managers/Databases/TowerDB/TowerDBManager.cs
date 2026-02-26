using UnityEngine;

public class TowerDBManager : Singleton<TowerDBManager>
{
    [SerializeField] private TowerDB dB;
    public TowerDB DB { get => dB; }

    private void Start()
    {
        Init();
        dB.Init();
    }
}
