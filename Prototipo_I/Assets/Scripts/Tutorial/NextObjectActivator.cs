using UnityEngine;

public class NextObjectActivator : MonoBehaviour
{
    [SerializeField] GameObject nextThing;

    private void OnDestroy()
    {
        if (nextThing != null)
        {
            nextThing.SetActive(true);
        }
    }
}
