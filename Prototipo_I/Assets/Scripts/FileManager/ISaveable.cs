using UnityEngine;
using UnityEngine.Events;

public interface ISaveable<T>
{
    public string File { get; }
    public T DataToSave { get; set; }
    public UnityEvent<float> SaveEvent { get; set; }
    public virtual void Save()
    {
        FileManager.SaveFile(File, DataToSave);
        Debug.Log("Saving to " + Application.persistentDataPath + "/" + File);
    }
}
