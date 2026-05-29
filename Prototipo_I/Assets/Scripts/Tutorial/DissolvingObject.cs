using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DissolvingObject : MonoBehaviour
{

    [SerializeField] private List<DissolvedMaterial> materials;

    public void Dissolve()
    {
        for (int i = 0; i < materials.Count; i++)
            materials[i].GetComponent<DissolvedMaterial>().Dissolve();
    }
}
