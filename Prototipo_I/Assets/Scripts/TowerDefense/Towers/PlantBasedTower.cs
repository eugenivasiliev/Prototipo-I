using System.Collections;
using System.Collections.Generic;
using Audio;
using Combat;
using Enemies;
using Farm;
using Inventory;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace TowerDefense
{
    public class PlantBasedTower : Tower, IInteractable, IContexted
    {
        [SerializeField] private (PlantData data, float amount) currentPlant = (null, 0f);
        [SerializeField, Range(1, 20)] private int maxCapacity = 3;
        [SerializeField, Range(0, 1)] private float usesPerAttack = .2f;
        protected override bool CanAttack() => currentPlant.amount >= usesPerAttack;

        public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding> {
            new IInteractable.KeyBinding("refill_tower", InputActionChange.ActionCanceled, Action_Refill)
        };

        private void Start()
        {
            (this as IInteractable).Bind();
        }

        protected override void SpawnProjectile()
        {
            base.SpawnProjectile();
            currentPlant.amount -= usesPerAttack;
        }

        void Action_Refill(InputAction.CallbackContext context)
        {
            if(Inventory.Inventory.Instance.GetSeedCount() > maxCapacity - (int)Mathf.Floor(currentPlant.amount))
            {
                Inventory.Inventory.Instance.RemoveSeeds(maxCapacity - (int)Mathf.Floor(currentPlant.amount));
                currentPlant = (null, maxCapacity);
            }
        }

        public void OnInteract()
        {
            throw new System.NotImplementedException();
        }

        public bool ContextKeyActive() => !CanAttack();
    }
}