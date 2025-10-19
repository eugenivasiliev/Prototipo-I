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
        new IInteractable.KeyBinding("fire", InputActionChange.ActionCanceled, Action_Use)
    };
    public void OnInteract() {}

    public void Action_Use(InputAction.CallbackContext ctx)
    {
        //TODO: Spawn FirePlant
    }
}
