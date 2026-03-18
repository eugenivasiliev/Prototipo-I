using System;
using UnityEngine;

namespace Objectives
{
    public interface IObjective
    {
        void Init();

        public bool IsCompleted { get; }

        public string Text();
    }

    public abstract class Objective<T> : ScriptableObject, IObjective
    {
        public bool IsCompleted => CheckObjective();

        public abstract void UpdateObjective(T param);

        public abstract bool CheckObjective();

        string IObjective.Text() => Text;

        public abstract void Init();

        public abstract string Text { get; }
    }
}