using Audio;
using Combat;
using UnityEngine;

namespace Enemies
{
    public class Attack : EnemyState
    {
        public override void OnEnter()
        {
            enemy.Animator.SetBool("IsAttacking", true);
        }

        public override void OnExit()
        {
            enemy.Animator.SetBool("IsAttacking", false);
        }

        public override void Behaviour()
        {
            if (bb.targetTransform == null) return;

            float distance = Vector3.Distance(enemy.transform.position, bb.targetTransform.transform.position);
            if (distance < 10.45f)
            {
                AudioManager.Instance.PlaySFXEvent(enemy.AttackSound);
                ((IAttacker)enemy).Attack(bb.targetTransform.gameObject);
                enemy.SetState(EnemyAI.State.Chase);
            }
            else
                enemy.SetState(EnemyAI.State.Chase);

        }
    }
}