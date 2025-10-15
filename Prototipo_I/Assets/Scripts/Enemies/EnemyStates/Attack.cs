using UnityEngine;

public class Attack : EnemyState
{
    private IDamageable damageable;
    public override void Behaviour()
    {
        if(damageable != null)
        {
            damageable.Damage(enemy.Damage);
        }

        float minDistance = Mathf.Infinity;

        foreach (var plot in PlotManager.Instance.plots)
        {
            if (!plot.IsPlanted) continue;

            float distance = Vector3.Distance(enemy.transform.position, plot.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                damageable = plot.GetComponent<IDamageable>();
            }
        }
    }
}
