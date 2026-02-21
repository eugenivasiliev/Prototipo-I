using UnityEngine;
using UnityEngine.EventSystems;

public class TurretButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public TowerSpot spotReference;
    public float range;

    void Start()
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        spotReference.SetRange(0.0f);
        spotReference.ShowRange(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        spotReference.SetRange(15.0f);
        spotReference.ShowRange(true);
    }
}
