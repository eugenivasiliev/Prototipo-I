using System.Collections.Generic;
using System.Timers;
using Audio;
using Combat;
using Inventory;
using Items;
using Objectives;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Farm
{
    public class Plot : MonoBehaviour, IInteractable, IDamageable
    {
        [SerializeField] private HybridationManager hybridationManager;

        Plant plant;
        PlantData plantData;

        private bool isFertilized;
        private GameObject currentPlant;

        [SerializeField] private TextMeshProUGUI statusText;
        public TextMeshProUGUI StatusText => statusText;
        [SerializeField] public PlantData plantInfo;

        public bool IsPlanted { get { return plant != null; } }

        [SerializeField] private int health;
        public int Health { get => health; set => health = value; }
        public int MaxHealth { get => 100; set { } }


        public GameObject plantingFeedback;
        public GameObject ripeFeedback;
        private GameObject ripeParticles;
        public GameObject harvestFeedback;

        private void Awake()
        {
            if (statusText != null)
                statusText.gameObject.SetActive(false);

            health = 1;
        }

        public void Plant(PlantData data)
        {
            AudioManager.Instance.PlaySFX("Plant");
            if (IsPlanted)
            {
                if (hybridationManager.TryFindHybrid(plantData, data, out PlantData newPlant))
                {
                    this.plantData = newPlant;
                    plant = new Plant(newPlant);
                }
                else return;
            }
            else
            {
                plantData = data;
                plant = new Plant(data);
            }

            currentPlant = Instantiate(plantData.stages[0], transform.position, Quaternion.Euler(-90, 0, 0), transform);

            plant.OnStageChanged += OnPlantStageChanged;

            isFertilized = false;

            Instantiate(plantingFeedback, transform.position, Quaternion.identity, transform);
        }

        public void Fertilize()
        {
            if (!IsPlanted || isFertilized) return;
            AudioManager.Instance.PlaySFX("Fertilize");
            isFertilized = true;
            plant.ApplyFertilize(isFertilized);
        }
        private void Harvest()
        {
            if (!IsPlanted || !plant.IsFullyGrown) return;

            AudioManager.Instance.PlaySFX("Harvesting");
            Inventory.Inventory.Instance.AddItem(new GasPlantItem(), 3, out int amountDone);

            if (ObjectivesManager.Instance.TryGetObjective<PlantsCollected, int>(out List<PlantsCollected> objs))
            {
                foreach (PlantsCollected obj in objs)
                {
                    obj.UpdateObjective(3);
                }
            }

            Destroy(currentPlant);

            this.plant = null;
            currentPlant = null;
        }

        private void OnPlantStageChanged(int currentStage)
        {
            isFertilized = false;

            if (currentPlant != null) { Destroy(currentPlant); }

            AudioManager.Instance.PlaySFX("NextStage");
            GameObject prefab = plantData.stages[currentStage];
            currentPlant = Instantiate(prefab, transform.position, Quaternion.Euler(-90, 0, 0), transform);

        }

        public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding>{
            new IInteractable.KeyBinding("plant", InputActionChange.ActionCanceled, Action_Plant),
            //new IInteractable.KeyBinding("harvest", InputActionChange.ActionCanceled, Action_Harvest),
            //new IInteractable.KeyBinding("fertilize", InputActionChange.ActionCanceled, Action_Fertilize)
        };

        private void Action_Plant(InputAction.CallbackContext ctx)
        {
            //Item item = Inventory.Inventory.Instance.GetCurrentItem();
            //if (item != null && item is IPlantSeed)
            //{
            //    this.Plant((item as IPlantSeed).PlantData);
            //    Inventory.Inventory.Instance.RemoveItem(item);
            //    plant.TryGrow(DayNightCycle.Instance.TotalTime);
            //}

            if (Inventory.Inventory.Instance.GetSeedCount() > 0)
            {
                this.Plant(DBManager.Instance.PlantDB["GasPlant"]);
                Inventory.Inventory.Instance.RemoveSeeds(1);
                plant.TryGrow(DayNightCycle.Instance.TotalTime);
            }
        }

        private void Action_Harvest(InputAction.CallbackContext ctx)
        {
            if (IsPlanted && plant.IsFullyGrown)
                Harvest();

            Destroy(ripeParticles);
        }

        private void Action_Fertilize(InputAction.CallbackContext ctx)
        {
            Fertilize();
        }
        public void FullGrow()
        {
            if (plant != null)
                plant.FullGrow();
            else
                return;

            OnPlantStageChanged(plant.CurrentStage - 1);

            ripeParticles = Instantiate(ripeFeedback, transform.position, Quaternion.identity, transform);
        }
        public void OnInteract() { }
    }
}