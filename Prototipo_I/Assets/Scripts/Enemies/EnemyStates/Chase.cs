using TowerDefense;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class Chase : EnemyState
    {
        private float distanceThreshold => 10.45f;

        public override void Behaviour()
        {
            if (bb.targetTransform == null)
            {
                switch (bb.target)
                {
                    case EnemyAI.Target.Plots:
                        float minDistance = Mathf.Infinity;

                        foreach (var plot in bb.plots)
                        {
                            if (!plot.IsPlanted) continue;

                            float d = Vector3.Distance(enemy.transform.position, plot.transform.position);

                            if (d < minDistance)
                            {
                                minDistance = d;
                                bb.targetTransform = plot.transform;
                            }
                        }
                        break;
                    case EnemyAI.Target.Home:
                        //bb.targetTransform = bb.homeTransform;
                        bb.targetTransform = Base.instance.transform;
                        float _minDistance = 5.0f;

                        foreach (var plot in bb.plots)
                        {
                            if (!plot.IsPlanted) continue;

                            float d = Vector3.Distance(enemy.transform.position, plot.transform.position);

                            if (d < _minDistance)
                            {
                                minDistance = d;
                                bb.targetTransform = plot.transform;
                            }
                        }
                        break;
                    case EnemyAI.Target.Player:
                        bb.targetTransform = bb.playerController.transform;
                        break;
                }

                enemy.BB = this.bb;
            }

            if (bb.targetTransform == null) return;


            NavMeshPath path = new NavMeshPath();
            enemy.Agent.CalculatePath(bb.targetTransform.position, path);
            enemy.Agent.SetPath(path);

            float distToTarget = Vector3.Distance(enemy.transform.position, bb.targetTransform.position);
            if (distToTarget < distanceThreshold) enemy.SetState(EnemyAI.State.Attack);

        }
    }
}