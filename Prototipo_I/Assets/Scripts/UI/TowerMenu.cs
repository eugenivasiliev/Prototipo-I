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
        private GridLayoutGroup towerMenuGrid;
        [SerializeField] private GameObject towerUI;

        [SerializeField] private PlayerController playerController;
        [SerializeField] private CameraControl cameraController;

        private bool isOpen = false;
        public bool IsOpen => isOpen;

        public TowerSpot spotReference = null;

        private float range;

        private void Awake()
        {
            towerMenuPanel.SetActive(false);
            towerMenuGrid = towerMenuPanel.GetComponentInChildren<GridLayoutGroup>();
        }

        private void LoadValidTowers()
        {
            foreach (Transform child in towerMenuGrid.transform) Destroy(child.gameObject);

            foreach (TowerData data in DBManager.Instance.TowerDB.filteredDatas[spotReference.TowerType])
            {
                if (Inventory.Inventory.Instance.HasSeeds(data.cost))
                {
                    GameObject instance = Instantiate(towerUI, towerMenuGrid.transform);
                    instance.GetComponent<Image>().sprite = data.uiSprite;
                    instance.GetComponent<Button>().onClick.AddListener(() => { spotReference.PlaceTower(data); });

                    TurretButton turretButton = instance.GetComponent<TurretButton>();
                    turretButton.spotReference = spotReference;
                    turretButton.range = data.range;
                    turretButton.tm = this;
                    turretButton.td = data;
                }
            }
        }

        public void ToggleMenu()
        {
            if (!isOpen && !Inventory.Inventory.Instance.HasSeeds()) return;

            isOpen = !isOpen;
            Cursor.lockState = (isOpen) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isOpen;
            towerMenuPanel.SetActive(isOpen);
            Time.timeScale = (isOpen) ? 0f : 1f;
            playerController.MovementLocked = isOpen;

            if (isOpen) LoadValidTowers();

            if (isOpen) {
                cameraController.forcedTweenMovement = true;
                cameraController.targetTweenPosition = cameraController.NearOffset;
                cameraController.SavePosition();
            } else
            {
                cameraController.forcedTweenMovement = true;
                cameraController.targetTweenPosition = cameraController.FarOffset;
            }
        }

        public void Exit()
        {
            isOpen = true;
            ToggleMenu();
        }
    }
}