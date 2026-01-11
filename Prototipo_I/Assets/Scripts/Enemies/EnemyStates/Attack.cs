using UnityEngine;

public class Attack : EnemyState
{
    private Plot target;
    public override void Behaviour()
    {
        if (target != null && target.IsPlanted)
        {
            float distance = Vector3.Distance(enemy.transform.position, target.transform.position);
            if (distance < 0.45f)
            {
                AudioManager.instance.PlaySFX("MonsterAttack");
                ((IAttacker)enemy).Attack(target.gameObject);
                enemy.SetState(EnemyAI.State.Chase);
            }
            else
                enemy.SetState(EnemyAI.State.Chase);
        }
        float minDistance = Mathf.Infinity;

        foreach (var plot in PlotManager.Instance.plots)
        {
            if (!plot.IsPlanted) continue;

            float distance = Vector3.Distance(enemy.transform.position, plot.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                target = plot;
            }
        }
    }
}
