using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : MonoBehaviour
{
    public static TowerMenu Instance { get; private set; }

    [SerializeField] private GameObject towerMenuPanel;
    [SerializeField] private GameObject towerUI;

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
        foreach (Transform child in this.transform.GetChild(0)) Destroy(child.gameObject);

        foreach (TowerData data in TowerDatabase.Instance.TowerDatas)
        {
            if(Inventory.Instance.HasIngredients(data.ingredients))
            {

                GameObject instance = Instantiate(towerUI, this.transform.GetChild(0));
                instance.GetComponent<Image>().sprite = ItemSpritesDatabase.SpriteDict.GetValueOrDefault(data.Name);
                instance.GetComponent<Button>().onClick.AddListener(() => { Debug.Log(name); spotReference.PlaceTower(data); });
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
