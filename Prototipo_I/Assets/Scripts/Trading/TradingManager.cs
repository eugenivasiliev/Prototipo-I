using System;
using System.Collections.Generic;
using Trading;
using UnityEngine;
using UnityEngine.InputSystem;

public class TradingManager : MonoBehaviour, IInteractable
{
    private static TradingManager instance;
    public static TradingManager Instance {  get { return instance; } }

    private List<Tuple<ITradeable, int>> stock = new List<Tuple<ITradeable, int>>(); //Item and amount

    private ITradeable weeklyObjective;
    public ITradeable WeeklyObjective { get { return weeklyObjective; } }

    [SerializeField] private GameObject tradingUI;

    public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding> { 
    new IInteractable.KeyBinding("trade", InputActionChange.ActionCanceled, ToggleTrading)
    };

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public bool TryBuy(ITradeable tradeable)
    {
        for (int i = 0; i < stock.Count; ++i)
            if (stock[i].Item1 == tradeable && PlayerController.Instance.Money >= stock[i].Item1.Price)
            {
                PlayerController.Instance.Money -= stock[i].Item1.Price;
                if (stock[i].Item2 > 1)
                    stock[i] = Tuple.Create(stock[i].Item1, stock[i].Item2 - 1);
                else
                    stock.RemoveAt(i);
                return true;
            }
        return false;
    }

    public bool TryBuy(ITradeable tradeable, int amount)
    {
        for (int i = 0; i < stock.Count; ++i)
            if (stock[i].Item1 == tradeable && PlayerController.Instance.Money >= stock[i].Item1.Price * amount)
            {
                PlayerController.Instance.Money -= stock[i].Item1.Price * amount;
                if (stock[i].Item2 > 1)
                    stock[i] = Tuple.Create(stock[i].Item1, stock[i].Item2 - 1);
                else
                    stock.RemoveAt(i);
                return true;
            }
        return false;
    }

    public void Sell(ITradeable tradeable)
    {
        for (int i = 0; i < stock.Count; ++i)
            if (stock[i].Item1 == tradeable)
            {
                stock[i] = Tuple.Create(stock[i].Item1, stock[i].Item2 + 1);
                return;
            }
        stock.Add(Tuple.Create(tradeable, 1));
    }

    public ITradeable GetNewWeeklyObjective()
    {
        throw new NotImplementedException();
    }

    public void OnInteract()
    {
        throw new NotImplementedException();
    }

    private void ToggleTrading(InputAction.CallbackContext ctx)
    {
        tradingUI.SetActive(!tradingUI.activeSelf);
    }
}
