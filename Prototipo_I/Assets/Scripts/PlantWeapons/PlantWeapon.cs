using UnityEngine;

namespace Combat
{
    public class PlantWeapon : MonoBehaviour
    {
        public virtual string Name { get => nameof(PlantWeapon); }

        protected virtual void Start() { }
        protected virtual void Update() { }
    }
}