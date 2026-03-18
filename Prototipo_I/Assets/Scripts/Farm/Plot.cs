using System.Collections.Generic;
using System.Timers;
using Audio;
using Combat;
using Inventory;
using Objectives;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils;

namespace Farm
{
    public class Plot : MonoBehaviour, IInteractable, IDamageable, IContexted
    {
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

        [SerializeField] private Canvas healthHolder;
        private void Awake()
        {
            if (statusText != null)
                statusText.gameObject.SetActive(false);

            health = 1;
        }

        public void Plant(PlantData data)
        {
            AudioManager.Instance.PlaySFX("Plant");
            
            plantData = data;
            plant = new Plant(data);

            currentPlant = Instantiate(plantData.stages[0], transform.position, Quaternion.Euler(-90, 0, 0), transform);

            plant.OnStageChanged += OnPlantStageChanged;

            isFertilized = false;

            Instantiate(plantingFeedback, transform.position, Quaternion.identity, transform);
        }

        private void OnPlantStageChanged(int currentStage)
        {
            isFertilized = false;

            if (currentPlant != null) { Destroy(currentPlant); }

            AudioManager.Instance.PlaySFX("NextStage");
            GameObject prefab = plantData.stages[currentStage];
            currentPlant = Instantiate(prefab, transform.position, Quaternion.Euler(-90, 0, 0), transform);

            Inventory.Inventory.Instance.AddSeeds(plantData.seedsPerRound);
            if (ObjectivesManager.Instance.TryGetObjective<PlantsCollected, int>(out List<PlantsCollected> objs))
                objs[0].UpdateObjective(plantData.seedsPerRound);
        }

        public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding>{
            new IInteractable.KeyBinding("plant", InputActionChange.ActionCanceled, Action_Plant)
        };

        private void Action_Plant(InputAction.CallbackContext ctx)
        {
            if (IsPlanted || Inventory.Inventory.Instance.GetSeedCount() == 0) return;

            this.Plant(DBManager.Instance.PlantDB["GasPlant"]);
            Inventory.Inventory.Instance.RemoveSeeds(1);
            plant.TryGrow(DayNightCycle.Instance.TotalTime);
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

        void UpdateLife()
        {

            healthHolder.gameObject.SetActive(true);
            healthHolder.gameObject.transform.GetChild(1).GetComponent<Image>().fillAmount = (this as IDamageable).HealthRatio;
        }

        public void OnDamage() {}

        public bool ContextKeyActive() => !IsPlanted;
    }
}