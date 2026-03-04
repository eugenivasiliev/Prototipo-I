using UnityEngine;
using System.Collections.Generic;

public class PlotManager : Singleton<PlotManager>
{
    public List<Plot> plots = new List<Plot>();
    [SerializeField] private HybridationManager hybridationManager;
    public HybridationManager HybridationManager {  get { return hybridationManager; } }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        if(plots.Count == 0)
        {
            plots.AddRange(gameObject.GetComponentsInChildren<Plot>());
        }
    }

    private void Update()
    {

    }


    public void FullGrow() {
        foreach (var plot in plots)
        {
            plot.FullGrow();
        }
    }
}
