using System.Collections.Generic;
using Audio;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace TowerDefense
{
    public class TowerSpot : MonoBehaviour, IInteractable
    {
        TowerData towerData;

        private GameObject currentTower;

        public bool hasTower { get { return towerData != null; } }

        private GameObject range;

        [SerializeField] private TowerMenu tm;


        private void Start()
        {
            //(this as IInteractable).Bind();

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
            //foreach (var ingredient in data.ingredients)
            //{
            //    Inventory.Inventory.Instance.RemoveItem(ingredient.itemName, ingredient.amount, out int amountDone);
            //}

            Inventory.Inventory.Instance.RemoveSeeds(data.cost);

            AudioManager.Instance.PlaySFX("Plant");
            if (hasTower) return;

            towerData = data;
            currentTower = Instantiate(towerData.stages[0], transform.position + new Vector3(0, 1.0f, 0), Quaternion.Euler(0, 0, 0), transform);

            float r = currentTower.GetComponent<Tower>().GetRange();
            SetRange(r);

            tm.ToggleMenu();
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
            tm.spotReference = this;
            tm.ToggleMenu();
        }

        public void SetRange(float dist)
        {
            range.transform.localScale = new Vector3(dist * 4, dist * 4, dist * 4);
        }

        public void ShowRange(bool bo)
        {
            range.SetActive(bo);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") ShowRange(true);
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player") ShowRange(false);
        }

        public void OnInteract() { }
    }
}