using UnityEngine;
using UnityEngine.Events;

public interface ISaveable<T>
{
    public string File { get; }
    public abstract T DataToSave();
    public UnityEvent<float> SaveEvent { get; set; }
    public virtual void Save()
    {
        T data = DataToSave();
        FileManager.SaveFile(File, data);
        Debug.Log("Saving to " + Application.persistentDataPath + "/" + File);
    }
}
