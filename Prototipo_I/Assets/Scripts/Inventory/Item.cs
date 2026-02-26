using System;
using UnityEngine;

[Serializable]
public class Item
{
    public readonly Sprite sprite;

    public virtual string Name => this.GetType().ToString();

    public string Id => Name;

    public string Description;

    public virtual void OnUse(GameObject gameObject) {}
}
