using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class TowerDescriptionPanel : MonoBehaviour
    {
        void Update()
        {
            this.GetComponent<RectTransform>().anchoredPosition = Mouse.current.position.ReadValue();
        }
    }
}