using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    private bool isReset = false;
    private float currentTimeForNextDay = 0.0f;
    private float maxTimeForNextDay = 2.0f;
    private static PlayerController instance;
    public static PlayerController Instance { get { return instance; } }

    [Header("Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private KeyCode attackKey;
    [SerializeField, Range(0, 3)] private float attackCooldown = 0.6f;
    private float currentCooldown = 0.0f;

    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 7.5f;
    [SerializeField] private float cameraSensibility = 7.5f;
    [SerializeField] private float gravity = 9.80665f;
    [SerializeField] private float jumpHeight = 2f;

    [Header("Transforms")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform modelTransform;
    [SerializeField] public short InteractionRange { get { return 3; } }

    [Header("Camera")]
    [SerializeField] private Vector3 farCameraOffset;
    [SerializeField] private Vector3 nearCameraOffset;
    private Vector3 cameraOffset;
    private bool isCameraFar = true;
    [SerializeField] private AnimationCurve animationCurveX;
    [SerializeField] private AnimationCurve animationCurveY;
    [SerializeField, Range(0, 1)] private float animationDuration;

    [Header("VFX")]
    [SerializeField] private GameObject stunParticles;

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

    private bool waveMenuTouched = false;


    private Camera cam;
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

        cam = Camera.main;
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

        inputs.Player.Countdown.performed += ctx => OpenWaveMenu();
        //inputs.Player.Countdown.performed += ctx => StartCoroutine(NextDayCountdown());
        //inputs.Player.Countdown.canceled += ctx => ResetDayCountdown();

        inputs.Player.camera_zoom.performed += ToggleCameraDistance;

        inputs.Player.debug.performed += ctx => Stun(1);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraOffset = farCameraOffset;
        isCameraFar = true;
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
    }

    void Update()
    {
        Look();
        Movement();
        AttackLoop();
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
        Quaternion q = Quaternion.AngleAxis(mouseX, Vector3.up);
        cameraOffset = q * cameraOffset;
        farCameraOffset = q * farCameraOffset;
        nearCameraOffset = q * nearCameraOffset;

        cameraTransform.transform.position = transform.position + cameraOffset;

        cameraTransform.LookAt(transform.position, Vector3.up);

        cameraOffset -= new Vector3(0.0f, Input.GetAxisRaw("Mouse ScrollWheel") * 20, 0.0f);
    }

    void ToggleCameraDistance(InputAction.CallbackContext ctx)
    {
        StartCoroutine(ToggleAnim());
    }

    private IEnumerator ToggleAnim()
    {
        movementLocked = true;

        Vector3 startPos = (isCameraFar) ? farCameraOffset : nearCameraOffset;
        Vector3 endPos = (isCameraFar) ? nearCameraOffset : farCameraOffset;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / animationDuration;
            float alphaX = animationCurveX.Evaluate(t);
            float alphaY = animationCurveY.Evaluate(t);

            cameraOffset.x = (1 - alphaX) * startPos.x + alphaX * endPos.x;
            cameraOffset.y = (1 - alphaY) * startPos.y + alphaY * endPos.y;
            cameraTransform.position = transform.position + cameraOffset;
            cameraTransform.LookAt(transform.position, Vector3.up);

            yield return new WaitForEndOfFrame();
        }

        isCameraFar = !isCameraFar;
        movementLocked = false;

        yield return null;
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

    void OpenWaveMenu() {

        if (waveMenuTouched)
            WaveManager.Instance.ToggleWaveUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            closeEnemies.Add(other.gameObject);
        }
        else if (other.CompareTag("Console")) 
        {
            waveMenuTouched = true;
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
        else if (other.CompareTag("Console"))
        {
            waveMenuTouched = false;
        }
    }

    private void AttackLoop()
    {

        if (currentCooldown < attackCooldown) currentCooldown += Time.deltaTime;

        if (currentCooldown < attackCooldown || !Input.GetKey(attackKey) || !GetClosestEnemy()) return;
        
        SpawnProjectile(attackCooldown);
        DamageTarget();

        currentCooldown = 0;
    }

    private bool GetClosestEnemy()
    {
        closeEnemies.RemoveAll(item => item == null);
        targetedEnemy = (closeEnemies.Count > 0) ? closeEnemies[0] : null;
        return targetedEnemy != null;
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
        p.GetComponent<Projectile>().finalPos = targetedEnemy.transform;
        p.GetComponent<Projectile>().maxTime = waitTime;
    }

    public void Stun(float seconds)
    {
        //TODO: Add proper VFX/SFX

        Instantiate(stunParticles, transform.position, Quaternion.identity, transform);
        //AudioManager.instance.PlaySFX("Stun");

        StartCoroutine(StunCorroutine(seconds));
    }

    private IEnumerator StunCorroutine(float seconds) {
        movementLocked = true;

        yield return new WaitForSeconds(seconds);

        movementLocked = false;
    }
}
