using UnityEngine;
using System.Collections.Generic;

public class PlotManager : MonoBehaviour
{
    
    public static PlotManager Instance;
    public List<Plot> plots = new List<Plot>();

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
        foreach (var plot in plots)
        {
            plot.UpdateUI();
        }
    }
}
