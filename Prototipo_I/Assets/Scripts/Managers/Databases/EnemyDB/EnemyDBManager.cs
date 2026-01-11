using System;
using UnityEngine;

public class EnemyDBManager : DBManager<EnemyDB>
{

    protected override void Start()
    {
        base.Start();
        DB.Init();
    }
}
