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

        void Start()
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ResetRange();


            tm.EraseTowerDescription();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            spotReference.SetRange(15.0f);
            spotReference.ShowRange(true);

            tm.LoadTowerDescription(td);
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