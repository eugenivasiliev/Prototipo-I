using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : Singleton<ObjectivesManager>
{

    [SerializeField] private List<ScriptableObject> objectives = new List<ScriptableObject>();
    public List<ScriptableObject> Objectives => objectives;

    void Start()
    {
        InitSingleton();
        foreach (var obj in objectives)
            (obj as IObjective).Init();
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
