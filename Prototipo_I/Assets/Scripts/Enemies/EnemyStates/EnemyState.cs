using UnityEngine;

public abstract class EnemyState
{
    protected EnemyAI enemy;
    public EnemyAI Enemy { get => enemy; set => enemy = value; }
    public abstract void Behaviour();
}
