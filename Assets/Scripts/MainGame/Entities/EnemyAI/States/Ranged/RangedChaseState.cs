using UnityEngine;
using EnemyAI.Base;

namespace EnemyAI.States.Ranged
{
    public class RangedChaseState : State
    {
        private float stopDistance = 5f; // Distance to stop before attacking

        public RangedChaseState(EnemyBase _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void Enter()
        {
            enemy.agent.speed = enemy.chaseSpeed;
            enemy.animator.SetBool("isChasing", true);
        }

        public override void LogicUpdate()
        {
            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.target.position);

            // If within attack range, switch to attack state
            if (distanceToPlayer <= stopDistance)
            {
                stateMachine.ChangeState(enemy.AttackState);
                return;
            }

            // Otherwise, move toward the player
            enemy.agent.SetDestination(enemy.target.position);
        }

        public override void Exit()
        {
            enemy.animator.SetBool("isChasing", false);
        }
    }
}
