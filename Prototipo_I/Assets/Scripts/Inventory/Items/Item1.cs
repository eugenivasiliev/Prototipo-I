using System;
using System.Collections.Generic;
using Trading;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class Item1 : Item, IPlantSeed, ITradeable
{
    public PlantData PlantData => null;// PlantDatabase.Instance.GetPlantByName("Bulbasaur");
    public override string Name => "Item1";
    public int Price => 15;
}
