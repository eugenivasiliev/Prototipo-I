using UnityEngine;

public abstract class DBManager<T> : MonoBehaviour
{
    protected static DBManager<T> instance;
    public static DBManager<T> Instance { get { return instance; } }

    [SerializeField] private T database;
    public T DB { get { return database; } }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(this.gameObject);
    }
}
