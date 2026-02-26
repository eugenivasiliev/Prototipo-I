using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlantBasedTower : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject projectile;
    bool attacking = false;
    private List<GameObject> closeEnemies = new List<GameObject>();
    private GameObject targetedEnemy;

    [SerializeField] private (PlantData data, float amount) currentPlant = (null, 0f);
    private readonly int maxCapacity = 3;
    private readonly float usesPerAttack = .2f;
    private bool CanAttack => currentPlant.amount >= usesPerAttack;

    public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding> { 
            new IInteractable.KeyBinding("refill_tower", InputActionChange.ActionCanceled, Action_Refill)
        };

    private void Start()
    {
        (this as IInteractable).Bind();
    }

    void Action_Refill(InputAction.CallbackContext context)
    {
        Item item = Inventory.Instance.GetCurrentItem();
        if (item != null && item is IPlantSeed)
        {
            if (currentPlant.data != (item as IPlantSeed).PlantData) currentPlant = (null, 0);
            Inventory.Instance.RemoveItem(item, maxCapacity - (int)Mathf.Floor(currentPlant.amount), out int amountDone);
            currentPlant = ((item as IPlantSeed).PlantData, (int)Mathf.Floor(currentPlant.amount) + amountDone);
        }
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

            if (closeEnemies.Count == 0)
            {

                attacking = false;
                targetedEnemy = null;
            }
        }
    }

    IEnumerator AttackLoop()
    {
        attacking = true;

        float waitTime = 0.6f;
        
        while (attacking && closeEnemies.Count > 0 && CanAttack)
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
        else
        {
            targetedEnemy = null;
            attacking = false;
        }
    }

    void DamageTarget()
    {
        if (targetedEnemy == null || !CanAttack) return;

        if (targetedEnemy.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.DamagePercent(50.0f);
            currentPlant.amount -= usesPerAttack;
        }
    }

    public void OnInteract()
    {
        throw new System.NotImplementedException();
    }
    
    void SpawnProjectile(float waitTime)
    {
        transform.LookAt(targetedEnemy.transform.position, Vector3.up);
        AudioManager.instance.PlaySFX("TurretVAttack");
        GameObject p = Instantiate(projectile, this.transform.position, this.transform.rotation);
        p.transform.rotation = Quaternion.Euler(180, p.transform.rotation.eulerAngles.y, p.transform.rotation.eulerAngles.z);
        p.GetComponent<Projectile>().startPos = transform.position;
        p.GetComponent<Projectile>().finalPos = targetedEnemy.transform;
        p.GetComponent<Projectile>().maxTime = waitTime;
    }
}
