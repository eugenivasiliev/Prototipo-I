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
    public class PlayerController : MonoBehaviour
    {
        [Header("Attack")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private KeyCode attackKey;
        [SerializeField, Range(0, 3)] private float attackCooldown = 0.6f;
        private float currentCooldown = 0.0f;

        [Header("Movement")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float sprintSpeed = 7.5f;
        [SerializeField] private float gravity = 9.80665f;
        [SerializeField] private Animator anim;

        [Header("Transforms")]
        [SerializeField] private Transform modelTransform;
        [SerializeField] public short InteractionRange { get { return 3; } }

        [Header("VFX")]
        [SerializeField] private GameObject stunParticles;

        private CharacterController characterController;
        private InputSystem_Actions inputs;
        public InputSystem_Actions Inputs { get { return inputs; } }

        private Vector2 movementInput;
        private Vector3 velocity;
        private Vector3 horizontalMovement;

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
            inputs.Player.Enable();
            inputs.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
            inputs.Player.Move.canceled += ctx => movementInput = Vector2.zero;
            inputs.Player.Sprint.performed += ctx => isSprinting = true;
            inputs.Player.Sprint.canceled += ctx => isSprinting = false;
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
            AttackLoop();
        }

        private void Movement()
        {
            if (movementLocked) return;

            Transform cameraTransform = Camera.main.transform;
            Vector3 cameraForwardProjected = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
            Vector3 cameraRightProjected = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;

            Vector3 movement = cameraRightProjected * movementInput.x + cameraForwardProjected * movementInput.y;
            float currentSpeed = isSprinting ? sprintSpeed : speed;

            if (characterController.isGrounded)
                horizontalMovement = movement * currentSpeed;

            if (characterController.isGrounded && velocity.y < 0)
                velocity.y = -2;

            velocity.y -= gravity * Time.deltaTime;

            Vector3 totalMovement = horizontalMovement + new Vector3(0, velocity.y, 0);
            characterController.Move(totalMovement * Time.deltaTime);

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
                SetAnimation(MovementType.IDLE);
                AudioManager.Instance.StopLoop("Running");
                return;
            }

            AudioManager.Instance.PlaySFXLoop("Running");

            if (Mathf.Abs(movementInput.x) > Mathf.Abs(movementInput.y))
            {
                //Right-Left axis
                if (movementInput.x > 0)
                    SetAnimation(MovementType.RIGHT);
                else
                    SetAnimation(MovementType.LEFT);
            } else
            {
                //Forward-Backward axis
                if (movementInput.y > 0)
                    SetAnimation(MovementType.FORWARD);
                else
                    SetAnimation(MovementType.BACKWARD);
            }
        }

        private void SetAnimation(MovementType direction)
        {
            anim.SetBool("Is_R_Front", false);
            anim.SetBool("Is_R_Backwards", false);
            anim.SetBool("Is_R_Right", false);
            anim.SetBool("Is_R_Left", false);

            switch (direction)
            {
                case MovementType.FORWARD:
                    anim.SetBool("Is_R_Front", true);
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
            targetedEnemy = (closeEnemies.Count > 0) ? closeEnemies[0] : null;
            return targetedEnemy != null;
        }

        void SpawnProjectile(float waitTime)
        {
            AudioManager.Instance.PlaySFX("PlayerAttack");
            GameObject p = Instantiate(projectilePrefab, this.transform.position, this.transform.rotation);
            Projectile projectile = p.GetComponent<Projectile>();
            projectile.startPos = transform.position;
            projectile.target = targetedEnemy;
        }

        public void Stun(float seconds)
        {
            //TODO: Add proper VFX/SFX

            Instantiate(stunParticles, transform.position, Quaternion.identity, transform);
            //AudioManager.instance.PlaySFX("Stun");

            StartCoroutine(StunCorroutine(seconds));
        }

        private IEnumerator StunCorroutine(float seconds)
        {
            movementLocked = true;

            yield return new WaitForSeconds(seconds);

            movementLocked = false;
        }
    }
}