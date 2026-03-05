using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UISpritesDB", menuName = "Scriptable Objects/Databases/UISprites")]
public class UISpritesDB : ScriptableObject
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    public Dictionary<string, Sprite> spritesWithName { get; private set; }

    public Sprite this[string s] => spritesWithName.GetValueOrDefault<string, Sprite>(s); 

    public void Init()
    {
        spritesWithName = new Dictionary<string, Sprite>();
        foreach (var sprite in sprites)
            spritesWithName.Add(sprite.name, sprite);
    }
}