using UnityEngine;
using EnemyAI.Base;
using EnemyAI.Enemies.Boss;

namespace EnemyAI.States.Boss
{
    public class BossChaseState : State
    {
        public BossChaseState(EnemyBase _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void Enter()
        {
            enemy.agent.speed = enemy.chaseSpeed * 0.7f; // Boss moves slower
            enemy.animator.SetBool("isChasing", true);
        }

        public override void LogicUpdate()
        {
            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.target.position);

            if (distanceToPlayer <= enemy.attackRange)
            {
                if (enemy is BossEnemy boss && boss.CanUseSpecialAttack)
                {
                    stateMachine.ChangeState(new BossSpecialAttackState(enemy, stateMachine));
                    return;
                }

                stateMachine.ChangeState(enemy.AttackState);
                return;
            }

            enemy.agent.SetDestination(enemy.target.position);
        }

        public override void Exit()
        {
            enemy.animator.SetBool("isChasing", false);
        }
    }
}
