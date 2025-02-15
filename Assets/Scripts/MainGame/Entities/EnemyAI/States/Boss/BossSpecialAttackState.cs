using UnityEngine;
using EnemyAI.Base;
using EnemyAI.Enemies.Boss;

namespace EnemyAI.States.Boss
{
    public class BossSpecialAttackState : State
    {
        public BossSpecialAttackState(EnemyBase _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log($"{enemy.name} enters Special Attack State!");

            // Call the special attack
            (enemy as BossEnemy)?.PerformSpecialAttack();

            enemy.animator.SetTrigger("SpecialAttack");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Vector3.Distance(enemy.transform.position, enemy.target.position) > enemy.attackRange)
            {
                stateMachine.ChangeState(enemy.ChaseState);
            }
        }
    }
}
