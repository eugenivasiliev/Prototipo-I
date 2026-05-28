using System.Collections;
using System.Collections.Generic;
using Audio;
using Combat;
using Enemies;
using TowerDefense;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Player
{
    public class PlayerController : Singleton<PlayerController>
    {
        [Header("Attack")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private KeyCode attackKey;
        [SerializeField, Range(0, 3)] private float attackCooldown = 0.6f;
        [SerializeField, Range(0, 180)] private float attackAngle = 30f; //In degrees
        [SerializeField] private GameObject attackCone;
        [SerializeField] private bool defaultArmed = false;
        private float currentCooldown = 0.0f;
        private bool isArmed = false;

        [Header("Movement")]
        [SerializeField] private float speed = 35f;        
        [SerializeField] private float gravity = 9.80665f;
        [SerializeField] private Animator anim;
        [SerializeField] private Vector3 targetForward;
        [SerializeField] private float rotationSpeed;

        [Header("Idle")]
        [SerializeField, Range(0, 60)] private float minIdleChangeSeconds;
        [SerializeField, Range(0, 60)] private float maxIdleChangeSeconds;
        private float randomIdleChangeSeconds = 0;
        private float currentIdleChangeSeconds = 0;

        [Header("Transforms")]
        [SerializeField] private Transform modelTransform;
        [SerializeField, Range(0, 10)] public float InteractionRange = 3;

        [Header("VFX")]
        [SerializeField] private GameObject stunParticles;

        private CharacterController characterController;
        private InputSystem_Actions inputs;
        public InputSystem_Actions Inputs { get { return inputs; } }

        private Vector2 movementInput;
        private Vector3 velocity;
        private Vector3 horizontalMovement;
        private MovementType curMovementType = MovementType.IDLE;

        private bool isSprinting;

        private List<GameObject> closeEnemies = new List<GameObject>();
        private GameObject targetedEnemy;
        int damage = 1;
        bool attacking = false;

        private bool movementLocked = false;
        public bool MovementLocked { get => movementLocked; set => movementLocked = value; }

        [SerializeField] private Image gunRecharge;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            if (inputs == null) inputs = new InputSystem_Actions();
        }

        private void Start()
        {
            InitSingleton();

            inputs.Player.Enable();
            inputs.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
            inputs.Player.Move.canceled += ctx => movementInput = Vector2.zero;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            anim.SetBool("Is_Armed", defaultArmed);
            attackCone.SetActive(defaultArmed);

            DayNightCycle.Instance.SubscribeTimedEvent(ToggleCone, 1);
        }

        private void OnDisable()
        {
            inputs.Player.Disable();
        }

        void Update()
        {
            Movement();
            AttackLoop();
        }

        private void Movement()
        {
            if (movementLocked) return;

            Transform cameraTransform = Camera.main.transform;
            Vector3 cameraForwardProjected = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
            Vector3 cameraRightProjected = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;

            Vector3 movement = cameraRightProjected * movementInput.x + cameraForwardProjected * movementInput.y;
            float currentSpeed = speed;

            if (characterController.isGrounded)
                horizontalMovement = movement * currentSpeed;

            if (characterController.isGrounded && velocity.y < 0)
                velocity.y = -2;

            velocity.y -= gravity * Time.deltaTime;

            Vector3 totalMovement = horizontalMovement + new Vector3(0, velocity.y, 0);
            characterController.Move(totalMovement * Time.deltaTime);

            targetForward = movement;

            if (movement.sqrMagnitude > 0)
            {
                float rotationAngle = Vector3.SignedAngle(modelTransform.forward, targetForward, Vector3.up);

                modelTransform.rotation =
                    Quaternion.AngleAxis(rotationAngle * Time.deltaTime * rotationSpeed, Vector3.up) * modelTransform.rotation;
            }

            Animate(movementInput);
        }

        public enum MovementType
        {
            FORWARD,
            BACKWARD,
            RIGHT,
            LEFT,
            IDLE
        }

        private void Animate(Vector2 movementInput)
        {
            if(movementInput.magnitude == 0)
            {
                currentIdleChangeSeconds += Time.deltaTime;

                anim.SetBool("Idle2", false);

                if (randomIdleChangeSeconds == 0)
                {
                    randomIdleChangeSeconds = Random.Range(minIdleChangeSeconds, maxIdleChangeSeconds);
                    currentIdleChangeSeconds = 0;
                }

                if (currentIdleChangeSeconds >= randomIdleChangeSeconds)
                {
                    randomIdleChangeSeconds = Random.Range(minIdleChangeSeconds, maxIdleChangeSeconds);
                    currentIdleChangeSeconds = 0;
                    anim.SetBool("Idle2", true);
                }

                if (curMovementType == MovementType.IDLE) return;
                SetAnimation(MovementType.IDLE);
                curMovementType = MovementType.IDLE;
                AudioManager.Instance.StopSFXLoop("EvelynFootstep");
                return;
            }

            randomIdleChangeSeconds = 0;

            AudioManager.Instance.PlaySFXLoop("EvelynFootstep");

            SetAnimation(MovementType.FORWARD);

            //if (Mathf.Abs(movementInput.x) > Mathf.Abs(movementInput.y))
            //{
            //    //Right-Left axis
            //    if (movementInput.x > 0)
            //    {
            //        if (curMovementType == MovementType.RIGHT) return;
            //        SetAnimation(MovementType.RIGHT);
            //        curMovementType = MovementType.RIGHT;
            //    }
            //    else
            //    {
            //        if (curMovementType == MovementType.LEFT) return;
            //        SetAnimation(MovementType.LEFT);
            //        curMovementType = MovementType.LEFT;
            //    }
            //} else
            //{
            //    //Forward-Backward axis
            //    if (movementInput.y > 0)
            //    {
            //        if (curMovementType == MovementType.FORWARD) return;
            //        SetAnimation(MovementType.FORWARD);
            //        curMovementType = MovementType.FORWARD;
            //    }
            //    else
            //    {
            //        if (curMovementType == MovementType.BACKWARD) return;
            //        SetAnimation(MovementType.BACKWARD);
            //        curMovementType = MovementType.BACKWARD;
            //    }
            //}
        }

        private void SetAnimation(MovementType direction)
        {
            anim.SetBool("Is_Walking", false);
            anim.SetBool("Is_R_Backwards", false);
            anim.SetBool("Is_R_Right", false);
            anim.SetBool("Is_R_Left", false);

            curMovementType = direction;

            switch (direction)
            {
                case MovementType.FORWARD:
                    anim.SetBool("Is_Walking", true);
                    break;
                case MovementType.BACKWARD:
                    anim.SetBool("Is_R_Backwards", true);
                    break;
                case MovementType.RIGHT:
                    anim.SetBool("Is_R_Right", true);
                    break;
                case MovementType.LEFT:
                    anim.SetBool("Is_R_Left", true);
                    break;
                default:
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<EnemyAI>(out EnemyAI enemyAI)) return;

            closeEnemies.Add(other.gameObject);
        }
        

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<EnemyAI>(out EnemyAI enemyAI)) return;
            
            closeEnemies.Remove(other.gameObject);
            if (closeEnemies.Count == 0)
            {
                attacking = false;
                targetedEnemy = null;
            }
        }

        private void AttackLoop()
        {
            Vector3 projectedFwd = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
            attackCone.transform.LookAt(attackCone.transform.position + projectedFwd);

            if (currentCooldown < attackCooldown) currentCooldown += Time.deltaTime;

            gunRecharge.fillAmount = currentCooldown / attackCooldown;

            if (currentCooldown < attackCooldown || !GetClosestEnemy()) return;

            SpawnProjectile(attackCooldown);
            gunRecharge.fillAmount = 0.0f;

            currentCooldown = 0;
        }

        private bool GetClosestEnemy()
        {
            closeEnemies.RemoveAll(item => item == null);
            targetedEnemy = null;

            for(int i = 0; i < closeEnemies.Count; ++i)
            {
                Vector3 enemyFwd = closeEnemies[i].transform.position - modelTransform.position;
                enemyFwd.y = 0;
                Vector3 attackFwd = attackCone.transform.forward;
                attackFwd.y = 0;
                if (Vector3.Angle(enemyFwd, attackFwd) > attackAngle / 2.0f)
                    continue;

                targetedEnemy = closeEnemies[i];
            }

            return targetedEnemy != null;
        }

        void SpawnProjectile(float waitTime)
        {
            AudioManager.Instance.PlaySFXEvent("EvelynShot");
            GameObject p = Instantiate(projectilePrefab, this.transform.position, this.transform.rotation);
            Projectile projectile = p.GetComponent<Projectile>();
            projectile.startPos = transform.position;
            projectile.target = targetedEnemy;
        }

        public void Stun(float seconds)
        {
            //TODO: Add proper VFX/SFX

            //Instantiate(stunParticles, transform.position, Quaternion.identity, transform);
            //AudioManager.instance.PlaySFX("Stun");

            StartCoroutine(StunCorroutine(seconds));
        }

        private IEnumerator StunCorroutine(float seconds)
        {
            movementLocked = true;

            yield return new WaitForSeconds(seconds);

            movementLocked = false;
        }

        private void ToggleCone(float t)
        {
            isArmed = !isArmed;
            anim.SetBool("Is_Armed", isArmed);
            attackCone.SetActive(isArmed);
            DayNightCycle.Instance.SubscribeTimedEvent(ToggleCone, 1);
        }
    }
}