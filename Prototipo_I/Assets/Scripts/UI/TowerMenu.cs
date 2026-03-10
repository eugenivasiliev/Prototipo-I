using System.Collections.Generic;
using Player;
using TMPro;
using TowerDefense;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class TowerMenu : MonoBehaviour
    {
        [SerializeField] private GameObject towerMenuPanel;
        [SerializeField] private GameObject towerMenuIngredients;
        [SerializeField] private GameObject towerUI;

        private bool isOpen = false;

        public bool IsOpen => isOpen;

        public TowerSpot spotReference = null;

        private float range;

        [Header("UI Sprites")]
        [SerializeField] private UISpritesDB uiSpritesDB;

        private void Awake()
        {
            towerMenuPanel.SetActive(false);
            towerMenuIngredients.SetActive(false);
        }

        private void LoadValidTowers()
        {
            foreach (Transform child in this.transform.GetChild(0)) Destroy(child.gameObject);

            foreach (TowerData data in DBManager.Instance.TowerDB.TowerDataList)
            {
                if (Inventory.Inventory.Instance.HasSeeds(data.cost))
                {

                    GameObject instance = Instantiate(towerUI, this.transform.GetChild(0));
                    instance.GetComponent<Image>().sprite = data.uiSprite;
                    instance.GetComponent<Button>().onClick.AddListener(() => { spotReference.PlaceTower(data); });

                    instance.GetComponent<TurretButton>().spotReference = spotReference;
                    instance.GetComponent<TurretButton>().range = data.range;
                    instance.GetComponent<TurretButton>().tm = this;
                    instance.GetComponent<TurretButton>().td = data;
                }
            }
        }

        public void LoadValidIngredients(TowerData td)
        {
            foreach (Transform child in this.transform.GetChild(1)) Destroy(child.gameObject);


            GameObject instance = Instantiate(towerUI, this.transform.GetChild(1));

            //instance.GetComponent<Image>().sprite = uiSpritesDB[td.ingredients[0].itemName];
            //instance.GetComponentInChildren<TMP_Text>().text = td.ingredients[0].amount.ToString();
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

            if (isOpen) LoadValidTowers();
        }

        public void Exit()
        {
            isOpen = true;
            ToggleMenu();
        }
    }
}