using Trading;
using UnityEngine;

public class TradingUI : MonoBehaviour
{
    [SerializeField] private Item itemToBuy;
    [SerializeField] private int amountToBuy;

    public void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void Buy()
    {
        if(TradingManager.Instance.TryBuy(itemToBuy as ITradeable, amountToBuy)) 
            Inventory.Instance.AddItem(itemToBuy, amountToBuy, out int amountBought);
    }

    public void Sell()
    {
        Item item = Inventory.Instance.GetCurrentItem();
        if (!(item is ITradeable)) return; //Cannot sell
        
        Inventory.Instance.RemoveItem(item);
        TradingManager.Instance.Sell(item as ITradeable);
    }
}
