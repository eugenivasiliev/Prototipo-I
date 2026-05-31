using TowerDefense;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class Chase : EnemyState
    {

        public override void Behaviour()
        {
            if (enemy.BB.targetTransform == null)
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
                        bb.targetTransform = bb.homeTransform;
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

                    case EnemyAI.Target.Barricade:
                        bb.targetTransform = bb.barricadeTransform;
                        break;
                }

                enemy.BB = this.bb;
            }

            if (enemy.BB.targetTransform == null) return;

            NavMeshPath path = new NavMeshPath();
            enemy.Agent.CalculatePath(enemy.BB.targetTransform.position, path);
            enemy.Agent.SetPath(path);

            bb = enemy.BB;
            bb.curAttackCooldown -= Time.deltaTime;
            enemy.BB = bb;
            if (enemy.BB.curAttackCooldown > 0) return;

            float distToTarget = Vector3.Distance(enemy.transform.position, enemy.BB.targetTransform.position);
            if (distToTarget < bb.attackRange) enemy.SetState(EnemyAI.State.Attack);

        }
    }
}