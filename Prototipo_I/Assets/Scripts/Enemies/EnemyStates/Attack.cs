using UnityEngine;

public class Attack : EnemyState
{
    private Plot target;
    public override void Behaviour()
    {
        AudioManager.instance.PlaySFX("MonsterAttack");
        if (target != null)
            ((IAttacker)enemy).Attack(target.gameObject);

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
