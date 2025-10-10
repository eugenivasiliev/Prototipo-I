using System;
using System.Collections.Generic;
using Trading;
using UnityEngine;

public class TradingManager : MonoBehaviour
{
    private static TradingManager instance;
    public static TradingManager Instance {  get { return instance; } }

    private List<Tuple<ITradeable, int>> stock = new List<Tuple<ITradeable, int>>(); //Item and amount

    private ITradeable weeklyObjective;
    public ITradeable WeeklyObjective { get { return weeklyObjective; } }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public bool TryBuy(ITradeable tradeable, int availableMoney)
    {
        foreach (var item in stock)
            if (item.Item1 == tradeable && availableMoney >= item.Item2) return true;
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
}
