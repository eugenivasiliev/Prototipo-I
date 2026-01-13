using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool isReset = false;
    private float currentTimeForNextDay = 0.0f;
    private float maxTimeForNextDay = 2.0f;
    private static PlayerController instance;
    public static PlayerController Instance { get { return instance; } }

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 7.5f;
    [SerializeField] private float cameraSensibility = 7.5f;
    [SerializeField] private float gravity = 9.80665f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform modelTransform;
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

    private List<GameObject> closeEnemies = new List<GameObject>();
    private GameObject targetedEnemy;
    int damage = 1;
    bool attacking = false;

    [SerializeField] private int money;
    public int Money { get => money; set => money = value; }

    private static bool movementLocked = false;
    public static bool MovementLocked { get => movementLocked; set => movementLocked = value; }


    private float currentTimeForNextDay = 0.0f;
    private float maxTimeForNextDay = 3.0f;
    private bool isReset = false;
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
        
        inputs.Player.Countdown.performed += ctx => StartCoroutine(NextDayCountdown());
        inputs.Player.Countdown.canceled += ctx => ResetDayCountdown();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
    }

    void Update()
    {
        Look();
        Movement();
    }

    private void Movement()
    {
        if(movementLocked) return;

        Vector3 cameraForwardProjected = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z);
        Vector3 cameraRightProjected = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z);

        Vector3 movement = cameraRightProjected * movementInput.x + cameraForwardProjected * movementInput.y;
        float currentSpeed = isSprinting ? sprintSpeed : speed;

        if (characterController.isGrounded)
            horizontalMovement = movement * currentSpeed;

        if (characterController.isGrounded && velocity.y < 0)
            velocity.y = -2;

        if(isJumping && characterController.isGrounded)
        {
            AudioManager.instance.PlaySFX("Jumping");
            velocity.y = Mathf.Sqrt(2f * jumpHeight * gravity);
            Debug.Log(velocity.y);
            isJumping = false;
        }

        velocity.y -= gravity * Time.deltaTime;

        Vector3 totalMovement = horizontalMovement + new Vector3(0, velocity.y, 0);
        characterController.Move(totalMovement * Time.deltaTime);

        if(horizontalMovement.sqrMagnitude > 0.01f)
        {
            modelTransform.LookAt(transform.position + cameraForwardProjected);
        }

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

        float mouseX = cameraInput.x* cameraSensibility;
        Vector3 offset = cameraTransform.position - transform.position;
        Quaternion q = Quaternion.AngleAxis(mouseX, Vector3.up);
        cameraTransform.transform.position = transform.position + q * offset;

        cameraTransform.LookAt(transform.position, Vector3.up);
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

    IEnumerator NextDayCountdown()
    {

        currentTimeForNextDay += Time.deltaTime;

        yield return null;

        if (currentTimeForNextDay >= maxTimeForNextDay && isReset == false)
        {
            //DayNightCycle.Instance.NextDay();
            currentTimeForNextDay = 0.0f;
            PlotManager.Instance.FullGrow();
        }
        else if (isReset == false)
            StartCoroutine(NextDayCountdown());
        else
            isReset = false;
    }


    private void ResetDayCountdown()
    {

        currentTimeForNextDay = 0.0f;
        isReset = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            
            closeEnemies.Add(other.gameObject);

            if (attacking == false)
                StartCoroutine(AttackLoop());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            closeEnemies.Remove(other.gameObject);

            if (closeEnemies.Count == 0) {

                attacking = false;
                targetedEnemy = null;
            }
        }
    }

    IEnumerator AttackLoop()
    {
        attacking = true;

        float waitTime = 0.6f;

        while (attacking && closeEnemies.Count > 0)
        {
            if (targetedEnemy == null)
                GetClosestEnemy();

            SpawnProjectile(waitTime);

            yield return new WaitForSeconds(waitTime);

            if (targetedEnemy != null)
                DamageTarget();
            else 
            {
                GetClosestEnemy();
                DamageTarget();
            }
        }

        attacking = false;
    }

    void GetClosestEnemy()
    {
        closeEnemies.RemoveAll(item => item == null);

        if (closeEnemies.Count > 0)
            targetedEnemy = closeEnemies[0];
        else {
            targetedEnemy = null;
            attacking = false;
        }
    }

    void DamageTarget()
    {
        if (targetedEnemy == null) return;

        if (targetedEnemy.TryGetComponent<IDamageable>(out var damageable))
            damageable.DamageMax();        
    }

    void SpawnProjectile(float waitTime) {
        AudioManager.instance.PlaySFX("PlayerAttack");
        GameObject p = Instantiate(projectilePrefab, this.transform.position, this.transform.rotation);
        p.GetComponent<Projectile>().startPos = transform.position;
        p.GetComponent<Projectile>().finalPos = targetedEnemy.transform.position;
        p.GetComponent<Projectile>().maxTime = waitTime;
    }
}
