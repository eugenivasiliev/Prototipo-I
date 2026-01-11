using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerMenu : MonoBehaviour
{
    public static TowerMenu Instance { get; private set; }

    [SerializeField] private GameObject towerMenuPanel;

    private bool isOpen = false;

    public bool IsOpen => isOpen;

    public TowerSpot spotReference = null;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        towerMenuPanel.SetActive(false);
    }

    private void LoadValidTowers()
    {
        foreach(TowerData data in TowerDatabase.Instance.TowerDatas)
        {
            if(Inventory.Instance.HasIngredients(data.ingredients))
            {
                Debug.Log(data.Name);
            }
        }
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        Cursor.lockState = (isOpen) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
        towerMenuPanel.SetActive(isOpen);
        Time.timeScale = (isOpen) ? 0f : 1f;
        PlayerController.MovementLocked = isOpen;

        if(isOpen) LoadValidTowers();
    }

    public void Exit()
    {
        isOpen = true;
        ToggleMenu();
    }
}
