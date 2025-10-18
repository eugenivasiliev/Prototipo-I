using UnityEngine;
using UnityEngine.Events;

public interface ISaveable<T>
{
    public string File { get; }
    public abstract T GetData();
    public abstract void SetData(T data);
    public UnityEvent<float> SaveEvent { get; set; }
    public virtual void Save()
    {
        T data = GetData();
        FileManager.SaveFile(File, data);
        Debug.Log("Saving to " + Application.persistentDataPath + "/" + File);
    }

    public virtual void Load()
    {
        FileManager.LoadFile(File, out T data);
        SetData(data);
    }
}
