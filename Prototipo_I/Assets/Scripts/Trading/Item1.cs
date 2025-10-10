using Trading;
using UnityEngine;

[CreateAssetMenu(fileName = "Item1", menuName = "Scriptable Objects/Item1")]
public class Item1 : ScriptableObject, ITradeable
{
    [SerializeField] private int price = 100;
    public int Price { get => price; set => price = value; }
}
