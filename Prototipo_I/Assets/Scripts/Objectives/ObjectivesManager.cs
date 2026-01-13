using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{

    public static ObjectivesManager Instance { get; private set; }

    [SerializeField] private List<ScriptableObject> objectives = new List<ScriptableObject>();
    public List<ScriptableObject> Objectives => objectives;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            foreach(var obj in objectives)
            {
                (obj as IObjective).Init();
            }
            return;
        }
        Destroy(this.gameObject);
    }

    void Update()
    {
        //if (AllObjectivesComplete())
        //    Debug.Log("done!");
    }

    public bool AllObjectivesComplete()
    {
        foreach (ScriptableObject objective in objectives)
            if(!(objective as IObjective).IsCompleted) return false;
        return true;
    }

    public bool TryGetObjective<Obj, T>(out List<Obj> obj) where Obj : Objective<T> 
    {
        obj = new List<Obj>();
        foreach(ScriptableObject objective in objectives)
            if (objective is Obj) obj.Add(objective as Obj);
        return obj.Count > 0;
    }
}
