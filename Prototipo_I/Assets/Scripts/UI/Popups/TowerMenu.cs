using System.Collections.Generic;
using Player;
using TMPro;
using TowerDefense;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class TowerMenu : MonoBehaviour
    {
        [SerializeField] private GameObject towerMenuPanel;
        private GridLayoutGroup towerMenuGrid;
        [SerializeField] private GameObject towerUI;
        [SerializeField] private GameObject towerInactiveUI;

        [SerializeField] private PlayerController playerController;
        [SerializeField] private CameraControl cameraController;

        [SerializeField] private SlidingPanelUI slidingPanel;

        [SerializeField] private TMP_Text controllerText;
        [SerializeField] private TMP_Text keyboardText;

        private bool isOpen = false;
        public bool IsOpen => isOpen;

        public TowerSpot spotReference = null;

        private float range;

        private enum InputType

        {
            KEYBOARD,
            CONTROLLER
        }

        private InputType inputType = InputType.KEYBOARD;

        private void Awake()
        {
            towerMenuGrid = towerMenuPanel.GetComponentInChildren<GridLayoutGroup>();
        }

        private void Update()
        {
            //keyboard keys & Mouse
            if (Input.anyKey)
                inputType = InputType.KEYBOARD;
            //joystick
            else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                inputType = InputType.CONTROLLER;

            controllerText.gameObject.SetActive(inputType == InputType.CONTROLLER);
            keyboardText.gameObject.SetActive(inputType == InputType.KEYBOARD);
        }

        private int MinCost()
        {
            int minCost = int.MaxValue;
            foreach (TowerData data in DBManager.Instance.TowerDB.filteredDatas[spotReference.TowerType])
                if(minCost >= data.cost)
                    minCost = data.cost;

            

            return minCost;
        }

        private void LoadValidTowers()
        {
            foreach (Transform child in towerMenuGrid.transform) Destroy(child.gameObject);
            bool isFirstButton = true;
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

                    if (Gamepad.current != null && isFirstButton)
                    {
                        EventSystem.current.SetSelectedGameObject(instance);
                        isFirstButton = false; 
                    }
                } else
                {
                    GameObject instance = Instantiate(towerInactiveUI, towerMenuGrid.transform);
                    instance.GetComponent<Image>().sprite = data.uiSprite;

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
            

            isOpen = !isOpen;
            Cursor.lockState = (isOpen) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isOpen;
            slidingPanel.Toggle();
            Time.timeScale = (isOpen) ? 0f : 1f;
            playerController.MovementLocked = isOpen;

            if (isOpen) LoadValidTowers();

            if (isOpen) {
                cameraController.forcedTweenMovement = true;
                cameraController.targetTweenPosition = cameraController.NearOffset;
                cameraController.SavePosition();
            } else
            {
                EventSystem.current.SetSelectedGameObject(this.gameObject);
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