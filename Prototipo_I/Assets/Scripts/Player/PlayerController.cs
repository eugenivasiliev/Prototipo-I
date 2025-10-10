using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IInteractable
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 7.5f;
    [SerializeField] private float cameraSensivility = 7.5f;
    [SerializeField] private float gravity = 9.80665f;
    [SerializeField] private float jumpHeaight = 2f;

    [SerializeField]private Transform cameraTransform;
    private CharacterController characterController;
    private InputSystem_Actions inputs;
    private Vector2 movementInput;
    private Vector2 cameraInput;
    private Vector3 velocity;
    private float Rotation;
    private bool isSprinting;
    private bool isJumping;
    private RaycastHit hit;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputs = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputs.Player.Enable();
        inputs.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputs.Player.Move.canceled += ctx => movementInput = Vector2.zero;
        inputs.Player.Look.performed += ctx => cameraInput = ctx.ReadValue<Vector2>();
        inputs.Player.Look.canceled += ctx => cameraInput = Vector2.zero;
        inputs.Player.Sprint.performed += ctx => isSprinting = true;
        inputs.Player.Sprint.canceled += ctx => isSprinting = false;
        inputs.Player.Interact.performed += ctx => 
        inputs.Player.Jump.performed += ctx => isJumping = true;
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    void Update()
    {
        Movement();
        Look();
        Interact();

    }

    private void Movement()
    {
        Vector3 movement = transform.right * movementInput.x + transform.forward * movementInput.y;
        float currentSpeed = isSprinting ? sprintSpeed : speed;
        characterController.Move(movement *  currentSpeed * Time.deltaTime);

        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        if(isJumping && characterController.isGrounded)
        {
            //Pedirle ayuda al @eauna
            //Pd: Mi objetivo es que la gravedad se aplique, es decir, que cuando llegue el Player a la altura maxima este empieze a bajar haciendo que su velocity.y sea igual a 0.
            //Utilizar Clamp con un float que represente la velocidad maxima.

            isJumping = false;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
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
        if (Physics.Raycast(transform.position, fwr, 10)) ;
           // Debug.Log("Yeah! I did it");




    }
}
