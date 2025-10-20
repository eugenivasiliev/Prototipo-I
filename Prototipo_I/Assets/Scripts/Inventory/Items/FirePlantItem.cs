using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class FirePlantItem : Item, IInteractable
{
    public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding>
    {
        new IInteractable.KeyBinding("Attack", InputActionChange.ActionCanceled, Action_Use)
    };
    public void OnInteract() {}

    public void Action_Use(InputAction.CallbackContext ctx)
    {
        GameObject.Instantiate(
            PlantWeaponsDatabase.Instance.GetPlantByName(nameof(FirePlant)),
            PlayerController.Instance.transform.position,
            Quaternion.identity
            );
        Inventory.Instance.RemoveItem(this);
    }
}
