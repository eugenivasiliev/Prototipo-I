using System;
using System.Collections.Generic;
using Trading;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class Item1 : Item, IPlantSeed, ITradeable
{
    public PlantData PlantData => PlantDatabase.Instance.GetPlantByName("Bulbasaur");

    public int Price => 15;
}
