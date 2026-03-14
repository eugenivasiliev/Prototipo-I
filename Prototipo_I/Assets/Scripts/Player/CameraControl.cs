using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
    public class CameraControl : TweenMovement
    {
        [SerializeField] private PlayerController player;

        [SerializeField] private Vector3 nearOffset;
        [SerializeField] private Vector3 farOffset;
        [SerializeField] private Vector3 currentOffset;
        [SerializeField] private Quaternion rotationOffset = Quaternion.identity;

        private InputSystem_Actions inputs;
        [SerializeField] private Vector2 lookInput;

        [Header("Settings")]
        [SerializeField] private float cameraSensibility = 7.5f;
        [SerializeField] private float scrollSensibility = 20.0f;

        override protected void Start()
        {
            inputs = PlayerController.Inputs;

            this.transform.position = player.transform.position + farOffset;
            currentOffset = farOffset;
            rotationOffset = Quaternion.identity;
            this.transform.LookAt(player.transform);

            inputs.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
            inputs.Player.Look.canceled += ctx => lookInput = Vector2.zero;

            xAxis.SetActive(true);
            yAxis.SetActive(true);
            zAxis.SetActive(true);
        }

        void Update()
        {
            //if (movementLocked) return;

            float mouseX = lookInput.x * cameraSensibility;
            Quaternion q = Quaternion.AngleAxis(mouseX, Vector3.up);
            rotationOffset *= q;

            float scroll = Mouse.current.scroll.ReadValue().y;

            if(scroll != 0f)
            {
                TweenUtil.Update(scroll * scrollSensibility, ref xAxis);
                TweenUtil.Update(scroll * scrollSensibility, ref yAxis);
                TweenUtil.Update(scroll * scrollSensibility, ref zAxis);
            }

            currentOffset = new Vector3(
                xAxis.value * nearOffset.x + (1 - xAxis.value) * farOffset.x,
                yAxis.value * nearOffset.y + (1 - yAxis.value) * farOffset.y,
                zAxis.value * nearOffset.z + (1 - zAxis.value) * farOffset.z
                );

            this.transform.position = player.transform.position + rotationOffset * currentOffset;
            this.transform.LookAt(player.transform.position, Vector3.up);
        }
    }
}