using System.Collections.Generic;
using Inventory;
using TMPro;
using Trading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class TradingUI : MonoBehaviour
    {
        [SerializeField] private Item itemToBuy;
        [SerializeField] private int amountToBuy;
        [SerializeField] private TMP_Text price;

        [SerializeField] private GameObject itemUI;

        private UnityAction<Item> setCurrentItem;

        public void Start()
        {
            this.gameObject.SetActive(false);
        }

        public void Buy()
        {
            if (!TradingManager.Instance.TryBuy(itemToBuy as ITradeable, amountToBuy)) return;

            Inventory.Inventory.Instance.AddItem(itemToBuy, amountToBuy, out int amountBought);
            UpdateVisuals();
        }

        public void Sell()
        {
            Item item = Inventory.Inventory.Instance.GetCurrentItem();
            if (!(item is ITradeable)) return; //Cannot sell

            Inventory.Inventory.Instance.RemoveItem(item);
            TradingManager.Instance.Sell(item as ITradeable);
            UpdateVisuals();
        }

        public void SetCurrentItem(Item item)
        {
            itemToBuy = item;
            price.text = (item as ITradeable).Price.ToString() + "€";
        }

        public void UpdateVisuals()
        {
            foreach (Transform child in this.transform.GetChild(1)) Destroy(child.gameObject);

            foreach (var itemInStock in TradingManager.Instance.Stock.items)
            {
                GameObject instance = Instantiate(itemUI, this.transform.GetChild(1));
                instance.GetComponent<Image>().sprite = (itemInStock.Tradeable as Item).sprite;
                Item item = itemInStock.Tradeable as Item;
                instance.GetComponent<Button>().onClick.AddListener(() => { SetCurrentItem(item); });
                instance.transform.GetComponentInChildren<TMP_Text>().text = itemInStock.amount.ToString();
            }
        }
    }
}