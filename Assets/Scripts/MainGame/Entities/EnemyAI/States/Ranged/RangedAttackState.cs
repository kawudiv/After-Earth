using EnemyAI.Base;
using EnemyAI.Components;
using EnemyAI.Enemies.Ranged;
using UnityEngine;

namespace EnemyAI.States.Ranged
{
    public class RangedAttackState : State
    {
        private float lastAttackTime;
        private float stopDistance = 5f; // Keeps distance from the player
        private EnemyCombat enemyCombat;

        public RangedAttackState(EnemyBase _enemy, StateMachine _stateMachine)
            : base(_enemy, _stateMachine) 
        {
            enemyCombat = _enemy.GetComponent<EnemyCombat>();
        }

        public override void Enter()
        {
            enemy.animator.SetTrigger("Attack");
            enemy.agent.isStopped = true;
            lastAttackTime = Time.time;
        }

        public override void LogicUpdate()
        {
            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.target.position);

            // If too far, chase the player again
            if (distanceToPlayer > stopDistance + 2f)
            {
                enemy.agent.isStopped = false;
                stateMachine.ChangeState(enemy.ChaseState);
                return;
            }

            // Attack if cooldown is over
            if (Time.time - lastAttackTime >= enemy.attackCooldown)
            {
                lastAttackTime = Time.time;
                enemy.animator.SetTrigger("Attack");
                enemyCombat?.PerformAttack();
            }
        }

        public override void Exit()
        {
            enemy.agent.isStopped = false;
        }
    }
}
