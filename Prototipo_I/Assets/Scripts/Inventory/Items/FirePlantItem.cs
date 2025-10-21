using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Trading;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class FirePlantItem : Item, IInteractable, ITradeable
{
    public override string spriteId => "FirePlant";

    public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding>
    {
        new IInteractable.KeyBinding("Attack", InputActionChange.ActionCanceled, Action_Use)
    };

    public int Price => 25;

    public void OnInteract() {}

    public void Action_Use(InputAction.CallbackContext ctx)
    {
        if (PlayerController.MovementLocked) return;
        GameObject instance = GameObject.Instantiate(
            PlantWeaponsDatabase.Instance.GetPlantByName(nameof(FirePlant)),
            PlayerController.Instance.transform.position,
            Quaternion.identity
            );
        instance.GetComponent<FirePlant>().animationStartPosition = PlayerController.Instance.transform.position;
        instance.GetComponent<FirePlant>().animationDirection = Camera.main.transform.forward;
        Inventory.Instance.RemoveItem(this);
    }
}
