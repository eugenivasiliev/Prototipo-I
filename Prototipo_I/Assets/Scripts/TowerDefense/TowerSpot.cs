using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSpot : MonoBehaviour, IInteractable
{
    Tower tower;
    TowerData towerData;

    private GameObject currentTower;

    public bool hasTower { get { return tower != null; } }

    private void Start()
    {
        (this as IInteractable).Bind();
    }

    public void PlaceTower(string dataName)
    {
        AudioManager.instance.PlaySFX("Plant");
        if (hasTower) return;

        towerData = TowerDatabase.Instance.GetTowerByName(dataName);
        tower = new Tower(towerData);
        currentTower = Instantiate(towerData.stages[0], transform.position + new Vector3(0, 1.0f, 0), Quaternion.Euler(0, 0, 0), transform);

        Debug.Log($"Tower {tower.Name} placed!");
    }

    public void PlaceTower(TowerData data)
    {
        foreach(var ingredient in data.ingredients)
        {
            Inventory.Instance.RemoveItem(ingredient.itemName, ingredient.amount, out int amountDone);
        }

        AudioManager.instance.PlaySFX("Plant");
        if (hasTower) return;

        towerData = data;
        tower = new Tower(data);
        currentTower = Instantiate(towerData.stages[0], transform.position + new Vector3(0, 1.0f, 0), Quaternion.Euler(0, 0, 0), transform);

        Debug.Log($"Tower {tower.Name} placed!");
    }

    private void OnTowerUpgraded(int level)
    {
        if (currentTower != null) { Destroy(currentTower); }

        AudioManager.instance.PlaySFX("NextStage");
        GameObject prefab = towerData.stages[level];
        currentTower = Instantiate(prefab, transform.position, Quaternion.Euler(-90, 0, 0), transform);

    }

    public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding>{
    new IInteractable.KeyBinding("place_tower", InputActionChange.ActionCanceled, Action_PlaceTower)
    };

    private void Action_PlaceTower(InputAction.CallbackContext context)
    {
        TowerMenu.Instance.spotReference = this;
        TowerMenu.Instance.ToggleMenu();
    }

    public void OnInteract() {}
}
