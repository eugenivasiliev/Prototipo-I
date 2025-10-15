using UnityEngine;

public class Chase : EnemyState
{
    private Plot targetPlot = null;
    public override void Behaviour()
    {
        if(targetPlot != null)
        {
            float distToTarget = Vector3.Distance(enemy.transform.position, targetPlot.transform.position);
            if (distToTarget < 1.5f)
                enemy.SetState(EnemyAI.State.Attack);
            return;
        }

        float minDistance = Mathf.Infinity;

        foreach (var plot in PlotManager.Instance.plots)
        {
            if (!plot.IsPlanted) continue;

            float distance = Vector3.Distance(enemy.transform.position, plot.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                targetPlot = plot;
            }
        }

        if (targetPlot != null)
        {
            enemy.Agent.SetDestination(targetPlot.transform.position);
        }
    }
}
