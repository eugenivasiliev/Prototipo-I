using System;
using UnityEngine;

public interface IObjective
{
    public bool IsCompleted { get; }
}

public abstract class Objective<T> : ScriptableObject, IObjective
{
    public bool IsCompleted => CheckObjective();

    public abstract void UpdateObjective(T param);

    public abstract bool CheckObjective();
}
