using UnityEngine;
using UnityEngine.EventSystems;

public class TurretButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public TowerSpot spotReference;
    public float range;

    public TowerMenu tm;
    public TowerData td;

    void Start()
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        spotReference.SetRange(0.0f);
        spotReference.ShowRange(false);


        tm.EraseValidIngredients();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        spotReference.SetRange(15.0f);
        spotReference.ShowRange(true);

        tm.LoadValidIngredients(td);
    }
}
