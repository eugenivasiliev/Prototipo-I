using UnityEngine;
using System.IO;
using System.Runtime.Serialization;

public static class FileManager
{
    public static string InventoryFile { get => "inventory.json"; }


    public static void SaveFile<T>(string fileName, T data)
    {
        string text = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/" + fileName;
        File.WriteAllText(path, text);
    }

    public static bool LoadFile<T>(string fileName, out T data) 
    {
        string path = Application.persistentDataPath + "/" + fileName;
        data = default;
        if (!File.Exists(path)) return false;

        string text = File.ReadAllText(path);
        data = JsonUtility.FromJson<T>(text);
        return true;
    }
}
