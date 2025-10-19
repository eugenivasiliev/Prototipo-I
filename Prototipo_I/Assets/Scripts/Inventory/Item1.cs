using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class Item1 : Item, IPlantSeed
{
    public PlantData PlantData => throw new NotImplementedException();
}
