using UnityEngine;

public class PlantWeaponsDatabase : MonoBehaviour
{
    private static PlantWeaponsDatabase instance;
    public static PlantWeaponsDatabase Instance { get { return instance; } }

    [SerializeField] private GameObject[] plantWeapons = new GameObject[0];
    public GameObject[] PlantWeapons { get { return plantWeapons; } }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public GameObject GetPlantByName(string name)
    {
        foreach (var plantData in plantWeapons)
            if (plantData.GetComponent<PlantWeapon>().Name == name) return plantData;

        return null;
    }
}
