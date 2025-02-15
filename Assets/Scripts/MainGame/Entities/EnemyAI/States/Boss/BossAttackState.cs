using UnityEngine;
using EnemyAI.Base;

namespace EnemyAI.States.Boss
{
    public class BossAttackState : State
    {
        private float attackCooldown = 3f;
        private float lastAttackTime;

        public BossAttackState(EnemyBase _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void Enter()
        {
            enemy.animator.SetTrigger("Attack");
            enemy.agent.isStopped = true;
            lastAttackTime = Time.time;
        }

        public override void LogicUpdate()
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                enemy.animator.SetTrigger("Attack");
            }

            if (Vector3.Distance(enemy.transform.position, enemy.target.position) > enemy.attackRange + 1f)
            {
                enemy.agent.isStopped = false;
                stateMachine.ChangeState(enemy.ChaseState);
            }
        }

        public override void Exit()
        {
            enemy.agent.isStopped = false;
        }
    }
}
