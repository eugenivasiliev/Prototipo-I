using UnityEngine;

public abstract class EnemyState
{
    protected EnemyAI enemy;
    public EnemyAI Enemy { get => enemy; set => enemy = value; }

    protected EnemyAI.Blackboard bb;
    public EnemyAI.Blackboard BB { get => bb; set => bb = value; }

    public abstract void Behaviour();
}
