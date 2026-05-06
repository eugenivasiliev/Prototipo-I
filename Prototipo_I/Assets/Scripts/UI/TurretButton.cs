using TMPro;
using TowerDefense;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

namespace UI
{
    public class TurretButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
        public TowerSpot spotReference;
        public float range;

        public TowerMenu tm;
        public TowerData td;
        [Header("Description")]
        public GameObject descriptionUI;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private GameObject cost;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private TMP_Text rangeText;
        [SerializeField, Range(0, 1000)] private float verticalOffset;
        [SerializeField, Range(0, 1000)] private float descriptionVerticalOffset;

        public void OnPointerExit(PointerEventData eventData)
        {
            ResetRange();
            this.GetComponent<RectTransform>().position -= Vector3.up * verticalOffset;
            descriptionUI.SetActive(false);
            cost.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            spotReference.SetRange(td.range);
            spotReference.SetDecal(td.Name + "Decal");
            spotReference.ShowRange(true);

            descriptionUI.SetActive(true);
            cost.SetActive(true);

            string localizedString = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizarionTableCollection", td.Name);

            descriptionText.text = localizedString;


            costText.text = td.cost.ToString();
            damageText.text = td.damage.ToString();
            rangeText.text = td.range.ToString();
            this.GetComponent<RectTransform>().position += Vector3.up * verticalOffset;
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