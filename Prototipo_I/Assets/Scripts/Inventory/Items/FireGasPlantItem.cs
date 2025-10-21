using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Trading;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class FireGasPlantItem : Item, IInteractable, ITradeable, IPlantSeed
{
    public override string spriteId => "FireGasPlant";

    public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding>
    {
        new IInteractable.KeyBinding("Attack", InputActionChange.ActionCanceled, Action_Use)
    };

    public PlantData PlantData => PlantDatabase.Instance.GetPlantByName("FireGas");
    public int Price => 25;

    public void OnInteract() {}

    public void Action_Use(InputAction.CallbackContext ctx)
    {
        if (PlayerController.MovementLocked) return;
        GameObject instance = GameObject.Instantiate(
            PlantWeaponsDatabase.Instance.GetPlantByName(nameof(FireGasPlant)),
            PlayerController.Instance.transform.position,
            Quaternion.Euler(-90, 0, 0)
            );
        instance.GetComponent<FireGasPlant>().animationStartPosition = PlayerController.Instance.transform.position;
        instance.GetComponent<FireGasPlant>().animationDirection = PlayerController.Instance.transform.forward;
        Inventory.Instance.RemoveItem(this);
    }
}
