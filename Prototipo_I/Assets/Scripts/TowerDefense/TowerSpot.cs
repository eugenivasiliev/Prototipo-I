using System.Collections.Generic;
using Audio;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace TowerDefense
{
    public class TowerSpot : MonoBehaviour, IInteractable, IContexted
    {

        [SerializeField] private TowerData towerData;

        [SerializeField] private GameObject currentTower;
        
        [SerializeField] private GameObject contextButton;

        public bool hasTower { get { return towerData != null; } }

        private GameObject range;

        [SerializeField] private TowerMenu tm;

        [SerializeField] private TowerData.TowerType towerType;
        public TowerData.TowerType TowerType { get { return towerType; } }

        private void Start()
        {
            if (this.transform.GetChild(0) != null)
                range = this.transform.GetChild(0).gameObject;
        }

        public void PlaceTower(string dataName)
        {
            AudioManager.Instance.PlaySFX("Plant");
            if (hasTower) return;

            towerData = DBManager.Instance.TowerDB[dataName];
            currentTower = Instantiate(towerData.stages[0], transform.position + new Vector3(0, 1.0f, 0), Quaternion.Euler(0, 0, 0), transform);
        }

        public void PlaceTower(TowerData data)
        {
            Inventory.Inventory.Instance.RemoveSeeds(data.cost);

            AudioManager.Instance.PlaySFX("Plant");
            if (hasTower) return;

            towerData = data;
            currentTower = Instantiate(towerData.stages[0], transform.position + new Vector3(0, 1.0f, 0), Quaternion.Euler(0, 0, 0), transform);

            if (currentTower.GetComponent<Tower>())
            {
                float r = currentTower.GetComponent<Tower>().GetRange();
                SetRange(r);
            }
            tm.ToggleMenu();
        
            Destroy(contextButton);
            Destroy(range);
        }

        private void OnTowerUpgraded(int level)
        {
            if (currentTower != null) { Destroy(currentTower); }

            AudioManager.Instance.PlaySFX("NextStage");
            GameObject prefab = towerData.stages[level];
            currentTower = Instantiate(prefab, transform.position, Quaternion.Euler(-90, 0, 0), transform);

        }

        public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding>{
            new IInteractable.KeyBinding("place_tower", InputActionChange.ActionCanceled, Action_PlaceTower)
        };

        private void Action_PlaceTower(InputAction.CallbackContext context)
        {
            if (hasTower) return;
            tm.spotReference = this;
            tm.ToggleMenu();
        }

        public void SetRange(float dist)
        {
            range.transform.localScale = new Vector3(dist * 2, dist * 2, dist * 2);
        }

        public void ShowRange(bool bo)
        {
            if (range)
            range.SetActive(bo);
        }

        private void OnTriggerEnter(Collider other)
        {
            //if (other.tag == "Player") ShowRange(true);
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player") ShowRange(false);
        }

        public void OnInteract() { }

        public bool ContextKeyActive() => !hasTower;
    }
}