using System;
using UnityEngine;

public class EnemyDBManager : DBManager<EnemyDB>
{

    protected override void Awake()
    {
        base.Awake();
        DB.Init();
    }
}
