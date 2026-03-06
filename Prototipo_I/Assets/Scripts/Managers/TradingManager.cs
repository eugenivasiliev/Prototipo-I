using System;
using System.Collections.Generic;
using Trading;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public struct StockItem
{
    [SerializeReference] public ITradeable Tradeable;
    public int amount;

    public StockItem(ITradeable tradeable, int amount)
    {
        Tradeable = tradeable;
        this.amount = amount;
    }

    public static StockItem Default = new StockItem(null, -1);
}

[Serializable]
public struct Stock
{
    public List<StockItem> items;

    public Stock(int length)
    {
        items = new List<StockItem>(length);
        for (int i = 0; i < length; ++i)
        {
            items[i] = StockItem.Default;
        }
    }
}

public class TradingManager : Singleton<TradingManager>, IInteractable, IAutoSaving<Stock>
{
    private Stock stock = new Stock(0); //Item and amount
    public Stock Stock { get { return stock; } }

    private ITradeable weeklyObjective;
    public ITradeable WeeklyObjective { get { return weeklyObjective; } }

    [SerializeField] private GameObject tradingUI;

    public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding> {
    new IInteractable.KeyBinding("trade", InputActionChange.ActionCanceled, ToggleTrading)
    };

    public float AutoSaveTime => 5f;

    public string File => "stock.json";

    public UnityEvent<float> SaveEvent { get; set; }

    public Stock DefaultData => new Stock(0);

    private void Start()
    {
        InitSingleton();

        (this as IAutoSaving<Stock>).SetupAutoSave();
        (this as IAutoSaving<Stock>).Load();
    }

    public bool TryBuy(ITradeable tradeable)
    {
        for (int i = 0; i < stock.items.Count; ++i)
            if (stock.items[i].Tradeable.GetType() == tradeable.GetType() &&
                PlayerController.Instance.Money >= stock.items[i].Tradeable.Price)
            {
                PlayerController.Instance.Money -= stock.items[i].Tradeable.Price;
                if (stock.items[i].amount > 1)
                    stock.items[i] = new StockItem(stock.items[i].Tradeable, stock.items[i].amount - 1);
                else
                    stock.items.RemoveAt(i);
                return true;
            }
        return false;
    }

    public bool TryBuy(ITradeable tradeable, int amount)
    {
        for (int i = 0; i < stock.items.Count; ++i)
            if (stock.items[i].Tradeable.GetType() == tradeable.GetType() &&
                PlayerController.Instance.Money >= stock.items[i].Tradeable.Price * amount)
            {
                PlayerController.Instance.Money -= stock.items[i].Tradeable.Price * amount;
                if (stock.items[i].amount > 1)
                    stock.items[i] = new StockItem(stock.items[i].Tradeable, stock.items[i].amount - 1);
                else
                    stock.items.RemoveAt(i);
                return true;
            }
        return false;
    }

    public void Sell(ITradeable tradeable)
    {
        for (int i = 0; i < stock.items.Count; ++i)
            if (stock.items[i].Tradeable.GetType() == tradeable.GetType())
            {
                PlayerController.Instance.Money += stock.items[i].Tradeable.Price;
                stock.items[i] = new StockItem(stock.items[i].Tradeable, stock.items[i].amount + 1);
                return;
            }
        stock.items.Add(new StockItem(tradeable, 1));
    }

    public ITradeable GetNewWeeklyObjective()
    {
        throw new NotImplementedException();
    }

    public void OnInteract() { }

    private void ToggleTrading(InputAction.CallbackContext ctx)
    {
        tradingUI.SetActive(!tradingUI.activeSelf);
        Cursor.visible = tradingUI.activeSelf;
        Cursor.lockState = (tradingUI.activeSelf) ? CursorLockMode.None : CursorLockMode.Locked;
        PlayerController.MovementLocked = tradingUI.activeSelf;
        if (tradingUI.activeSelf) tradingUI.GetComponent<TradingUI>().UpdateVisuals();
    }

    public Stock GetData() => stock;

    public void SetData(Stock data) => stock = data;
}