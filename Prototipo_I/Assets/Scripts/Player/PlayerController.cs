using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IInteractable
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 7.5f;
    [SerializeField] private float cameraSensivility = 7.5f;
    [SerializeField] private float gravity = 9.80665f;
    [SerializeField] private float jumpHeaight = 2f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] public short InteractionRange { get { return 10; } }

    private CharacterController characterController;
    private InputSystem_Actions inputs;

    private Vector2 movementInput;
    private Vector2 cameraInput;
    private Vector3 velocity;
    private Vector3 horizontalMovement;

    private float Rotation;
    private bool isSprinting;
    private bool isJumping;

    private RaycastHit hit;

    private IInteractable interactable;

    private HungerManager hungerManager;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputs = new InputSystem_Actions();
        hungerManager = GetComponent<HungerManager>();
    }

    private void Start()
    {
        inputs.Player.Enable();
        inputs.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputs.Player.Move.canceled += ctx => movementInput = Vector2.zero;
        inputs.Player.Look.performed += ctx => cameraInput = ctx.ReadValue<Vector2>();
        inputs.Player.Look.canceled += ctx => cameraInput = Vector2.zero;
        inputs.Player.Sprint.performed += ctx => isSprinting = true;
        inputs.Player.Sprint.canceled += ctx => isSprinting = false;
        inputs.Player.Interact.canceled += ctx => Interact();
        inputs.Player.Jump.performed += ctx => isJumping = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
    }

    void Update()
    {
        Movement();
        Look();
    }

    private void Movement()
    {
        Vector3 movement = transform.right * movementInput.x + transform.forward * movementInput.y;
        float currentSpeed = (isSprinting && hungerManager.CanSprint) ? sprintSpeed : speed;

        if (characterController.isGrounded)
            horizontalMovement = movement * currentSpeed;

        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        if(isJumping && characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(2f * jumpHeaight * gravity);
            Debug.Log(velocity.y);
            isJumping = false;
        }

        velocity.y -= gravity * Time.deltaTime;

        Vector3 totalMovement = horizontalMovement + new Vector3(0, velocity.y, 0);
        characterController.Move(totalMovement * Time.deltaTime);
    }

    private void Look()
    {
        float mouseX = cameraInput.x * cameraSensivility;
        float mouseY = cameraInput.y * cameraSensivility;

        Rotation -= mouseY;
        Rotation = Mathf.Clamp(Rotation, -90, 90);

        cameraTransform.localRotation = Quaternion.Euler(Rotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Interact()
    {
        Vector3 fwr = Camera.main.transform.forward;
        if (Physics.Raycast(transform.position, fwr, out hit, InteractionRange))
        {
            Debug.Log("Yeah! I did it");
            interactable = hit.collider.GetComponent<IInteractable>();
            if( interactable != null ) interactable.OnInteract();
        }
    }
}
