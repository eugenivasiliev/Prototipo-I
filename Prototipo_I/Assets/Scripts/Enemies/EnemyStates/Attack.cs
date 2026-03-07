using UnityEngine;

public class Attack : EnemyState
{
    public override void Behaviour()
    {
        if (bb.targetTransform == null) return;

        float distance = Vector3.Distance(enemy.transform.position, bb.targetTransform.transform.position);
        if (distance < 10.45f)
        {
            AudioManager.instance.PlaySFX("MonsterAttack");
            ((IAttacker)enemy).Attack(bb.targetTransform.gameObject);
            enemy.SetState(EnemyAI.State.Chase);
        }
        else
            enemy.SetState(EnemyAI.State.Chase);
        
    }
}
