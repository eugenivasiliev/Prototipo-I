using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private static PlayerController instance;
    public static PlayerController Instance { get { return instance; } }

    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 7.5f;
    [SerializeField] private float cameraSensivility = 7.5f;
    [SerializeField] private float gravity = 9.80665f;
    [SerializeField] private float jumpHeaight = 2f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] public short InteractionRange { get { return 10; } }

    private CharacterController characterController;
    private static InputSystem_Actions inputs;
    public static InputSystem_Actions Inputs { get { return inputs; } }

    private Vector2 movementInput;
    private Vector2 cameraInput;
    private Vector3 velocity;
    private Vector3 horizontalMovement;

    private float Rotation;
    private bool isSprinting;
    private bool isJumping;

    private RaycastHit hit;

    private IInteractable interactable;

    [SerializeField] private int money;
    public int Money { get => money; set => money = value; }

    private static bool movementLocked = false;
    public static bool MovementLocked { get => movementLocked; set => movementLocked = value; }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        characterController = GetComponent<CharacterController>();
        if(inputs == null) inputs = new InputSystem_Actions();
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
        if(movementLocked) return;

        Vector3 movement = transform.right * movementInput.x + transform.forward * movementInput.y;
        float currentSpeed = isSprinting ? sprintSpeed : speed;

        if (characterController.isGrounded)
            horizontalMovement = movement * currentSpeed;

        if (characterController.isGrounded && velocity.y < 0)
            velocity.y = -2;

        if(isJumping && characterController.isGrounded)
        {
            AudioManager.instance.PlaySFX("Jumping");
            velocity.y = Mathf.Sqrt(2f * jumpHeaight * gravity);
            Debug.Log(velocity.y);
            isJumping = false;
        }

        velocity.y -= gravity * Time.deltaTime;

        Vector3 totalMovement = horizontalMovement + new Vector3(0, velocity.y, 0);
        characterController.Move(totalMovement * Time.deltaTime);
        if (!isSprinting && movementInput.sqrMagnitude > 0.01f && characterController.isGrounded)
            AudioManager.instance.PlaySFXLoop("Walking");
        else
            AudioManager.instance.StopLoop("Walking");

        if (isSprinting && movementInput.sqrMagnitude > 0.01f && characterController.isGrounded)
            AudioManager.instance.PlaySFXLoop("Running");
        else
            AudioManager.instance.StopLoop("Running");
    }
    private void Look()
    {
        if(movementLocked) return;

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
