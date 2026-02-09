using UnityEngine;

public class UISpritesDBManager : Singleton<UISpritesDBManager>
{
    [SerializeField] private UISpritesDB dB;
    public UISpritesDB DB { get => dB; }

    private void Start()
    {
        Init();
        dB.Init();
    }
}
