using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantData", menuName = "Scriptable Objects/PlantData")]
public class PlantData : ScriptableObject
{
    public string plantName;
    public float timeToGrow;

    public GameObject[] stages;

    public GameObject plantWeapon;

}
