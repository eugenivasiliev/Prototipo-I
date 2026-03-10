using System;
using System.Collections.Generic;
using Combat;
using Farm;
using Inventory;
using NUnit.Framework.Constraints;
using Player;
using Trading;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Items
{
    [Serializable]
    public class FirePlantItem : Item, IInteractable, ITradeable, IPlantSeed
    {
        public override string Name => nameof(FirePlant);

        public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding>
    {
        new IInteractable.KeyBinding("Attack", InputActionChange.ActionCanceled, Action_Use)
    };

        public int Price => 25;

        public PlantData PlantData => DBManager.Instance.PlantDB[Name];

        public void OnInteract() { }

        public void Action_Use(InputAction.CallbackContext ctx)
        {
            if (PlayerController.MovementLocked) return;
            GameObject instance = GameObject.Instantiate(
                DBManager.Instance.PlantDB[Name].plantWeapon,
                PlayerController.Instance.transform.position,
                Quaternion.Euler(-90, 0, 0)
                );
            instance.GetComponent<FirePlant>().animationStartPosition = PlayerController.Instance.transform.position;
            instance.GetComponent<FirePlant>().animationDirection = PlayerController.Instance.transform.forward;
            Inventory.Inventory.Instance.RemoveItem(this);
        }
    }
}