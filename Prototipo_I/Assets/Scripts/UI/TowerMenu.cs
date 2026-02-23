using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : MonoBehaviour
{
    public static TowerMenu Instance { get; private set; }

    [SerializeField] private GameObject towerMenuPanel;
    [SerializeField] private GameObject towerMenuIngredients;
    [SerializeField] private GameObject towerUI;

    private bool isOpen = false;

    public bool IsOpen => isOpen;

    public TowerSpot spotReference = null;

    private float range;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        towerMenuPanel.SetActive(false);
        towerMenuIngredients.SetActive(false);
    }

    private void LoadValidTowers()
    {
        foreach (Transform child in this.transform.GetChild(0)) Destroy(child.gameObject);

        foreach (TowerData data in TowerDBManager.Instance.DB.TowerDataList)
        {
            if(Inventory.Instance.HasIngredients(data.ingredients))
            {

                GameObject instance = Instantiate(towerUI, this.transform.GetChild(0));
                instance.GetComponent<Image>().sprite = data.uiSprite;
                instance.GetComponent<Button>().onClick.AddListener(() => { Debug.Log(name); spotReference.PlaceTower(data); });

                instance.GetComponent<TurretButton>().spotReference = spotReference;
                instance.GetComponent<TurretButton>().range = data.range;
                instance.GetComponent<TurretButton>().tm = this;
                instance.GetComponent<TurretButton>().td = data;
            }
        }
    }

    //UISpritesDBManager
    public void LoadValidIngredients(TowerData td)
    {
        foreach (Transform child in this.transform.GetChild(1)) Destroy(child.gameObject);

        
        GameObject instance = Instantiate(towerUI, this.transform.GetChild(1));

        instance.GetComponent<Image>().sprite = UISpritesDBManager.Instance.DB[td.ingredients[0].itemName];
        instance.GetComponentInChildren<TMP_Text>().text = td.ingredients[0].amount.ToString();
    }
    
    public void EraseValidIngredients()
    {
        foreach (Transform child in this.transform.GetChild(1)) Destroy(child.gameObject);
    }    

        public void ToggleMenu()
    {
        isOpen = !isOpen;
        Cursor.lockState = (isOpen) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
        towerMenuPanel.SetActive(isOpen);
        towerMenuIngredients.SetActive(isOpen);
        Time.timeScale = (isOpen) ? 0f : 1f;
        PlayerController.MovementLocked = isOpen;

        EraseValidIngredients();

        if(isOpen) LoadValidTowers();
    }

    public void Exit()
    {
        isOpen = true;
        ToggleMenu();
    }        
}
