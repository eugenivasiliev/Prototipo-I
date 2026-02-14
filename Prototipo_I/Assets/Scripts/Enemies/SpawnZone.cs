using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [SerializeField] private List<int> validPhases = new List<int>();
    public List<int> ValidPhases { get { return validPhases; } }
}
