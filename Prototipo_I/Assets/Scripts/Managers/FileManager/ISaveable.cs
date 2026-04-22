using UnityEngine;
using UnityEngine.Events;

namespace Saving
{
    public interface ISaveable<T>
    {
        public string File { get; }
        public abstract T GetData();
        public abstract void SetData(T data);
        public System.Action<float> SaveEvent { get; set; }

        public T DefaultData { get; }

        public virtual void Save()
        {
            T data = GetData();
            FileManager.SaveFile(File, data);
        }

        public virtual void Load()
        {
            if (FileManager.LoadFile(File, out T data)) SetData(data);
            else SetData(DefaultData);
        }

        public virtual void LoadDefault()
        {
            SetData(DefaultData);
        }
    }
}
