using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using Player;

namespace Utils
{
    public interface IInteractable
    {

        public struct KeyBinding
        {
            public string actionName;
            public InputActionChange actionChange;
            public Action<InputAction.CallbackContext> action;

            public KeyBinding(string actionName, InputActionChange actionChange, Action<InputAction.CallbackContext> action)
            {
                this.actionName = actionName;
                this.actionChange = actionChange;
                this.action = action;
            }
        }

        public List<KeyBinding> keyBindings { get; }
        public abstract void OnInteract();
        public void Bind()
        {
            foreach (var binding in keyBindings)
            {
                switch (binding.actionChange)
                {
                    case InputActionChange.ActionStarted:
                        InputSystem.actions.FindAction(binding.actionName).started += binding.action;
                        break;
                    case InputActionChange.ActionCanceled:
                        InputSystem.actions.FindAction(binding.actionName).canceled += binding.action;
                        break;
                    case InputActionChange.ActionPerformed:
                        InputSystem.actions.FindAction(binding.actionName).performed += binding.action;
                        break;
                    default:
                        throw new Exception("Unexpected input binding ActionChange");
                }
            }
        }

        public void Unbind()
        {
            foreach (var binding in keyBindings)
            {
                switch (binding.actionChange)
                {
                    case InputActionChange.ActionStarted:
                        InputSystem.actions.FindAction(binding.actionName).started -= binding.action;
                        break;
                    case InputActionChange.ActionCanceled:
                        InputSystem.actions.FindAction(binding.actionName).canceled -= binding.action;
                        break;
                    case InputActionChange.ActionPerformed:
                        InputSystem.actions.FindAction(binding.actionName).performed -= binding.action;
                        break;
                    default:
                        throw new Exception("Unexpected input binding ActionChange");
                }
            }
        }

        public bool HasKeybinding(string bindingName)
        {
            foreach (var binding in keyBindings)
                if (binding.actionName == bindingName) return true;
            return false;
        }
    }
}