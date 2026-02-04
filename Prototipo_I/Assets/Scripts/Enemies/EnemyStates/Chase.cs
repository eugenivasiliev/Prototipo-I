using UnityEngine;

public class Chase : EnemyState
{
    private Plot targetPlot = null;
    public override void Behaviour()
    {
        if(targetPlot != null && targetPlot.IsPlanted)
        {
            float distToTarget = Vector3.Distance(enemy.transform.position, targetPlot.transform.position);
            if (distToTarget < 0.45f)
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
        //AudioManager.instance.PlaySFXLoop("MonsterWalking");
        if (targetPlot != null)
            enemy.Agent.SetDestination(targetPlot.transform.position);
    }
}
