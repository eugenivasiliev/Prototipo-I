using TowerDefense;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class TurretButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
        public TowerSpot spotReference;
        public float range;

        public TowerMenu tm;
        public TowerData td;
        public GameObject descriptionUI;
        [SerializeField, Range(0, 1000)] private float descriptionVerticalOffset;

        public void OnPointerExit(PointerEventData eventData)
        {
            ResetRange();
            tm.EraseTowerDescription();
            descriptionUI.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            spotReference.SetRange(td.range);
            spotReference.SetDecal(td.Name + "Decal");
            spotReference.ShowRange(true);

            tm.LoadTowerDescription(td);

            descriptionUI.SetActive(true);
            descriptionUI.GetComponent<RectTransform>().position = 
                this.GetComponent<RectTransform>().position + Vector3.up * descriptionVerticalOffset;
        }

        private void OnDisable()
        {
            if (spotReference != null)
            {
                spotReference.ShowRange(false);
            }
        }


        private void ResetRange()
        {
            if (spotReference != null)
            {
                spotReference.SetRange(0.0f);
                spotReference.ShowRange(false);
            }
        }
    }
}