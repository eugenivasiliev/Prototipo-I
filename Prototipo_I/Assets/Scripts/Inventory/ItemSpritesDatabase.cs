using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpritesDatabase : MonoBehaviour
{
    private static ItemSpritesDatabase instance;

    [Serializable]
    public struct SpriteWithId
    {
        public string Id;
        public Sprite Sprite;
    }

    [SerializeField] private List<SpriteWithId> sprites = new List<SpriteWithId>();

    private static Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
    public static Dictionary<string, Sprite> SpriteDict { get { return spriteDict; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            return;
        }

        instance = this;

        spriteDict.Clear();
        foreach (var sprite in sprites)
            spriteDict.Add(sprite.Id, sprite.Sprite);
        sprites.Clear(); //Free up the memory
    }

}
