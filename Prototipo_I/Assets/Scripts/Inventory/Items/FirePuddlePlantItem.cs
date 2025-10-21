using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Trading;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class FirePuddlePlantItem : Item, IInteractable, ITradeable
{
    public override string spriteId => "FirePuddlePlant";

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
            PlantWeaponsDatabase.Instance.GetPlantByName(nameof(FirePuddlePlant)),
            PlayerController.Instance.transform.position,
            Quaternion.Euler(-90, 0, 0)
            );
        instance.GetComponent<FirePuddlePlant>().animationStartPosition = PlayerController.Instance.transform.position;
        instance.GetComponent<FirePuddlePlant>().animationDirection = PlayerController.Instance.transform.forward;
        Inventory.Instance.RemoveItem(this);
    }
}
