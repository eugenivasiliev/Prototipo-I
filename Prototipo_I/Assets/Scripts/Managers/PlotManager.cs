using UnityEngine;
using System.Collections.Generic;

public class PlotManager : MonoBehaviour
{
    
    public static PlotManager Instance { get; private set; }
    public List<Plot> plots = new List<Plot>();
    [SerializeField] private HybridationManager hybridationManager;
    public HybridationManager HybridationManager {  get { return hybridationManager; } }

    private void Awake()
    {
        if(Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
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
