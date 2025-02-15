using EnemyAI.Base;
using UnityEngine;

namespace EnemyAI.States.Melee
{
    public class MeleeChaseState : State
    {
        public MeleeChaseState(EnemyBase _enemy, StateMachine _stateMachine)
            : base(_enemy, _stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log($"{enemy.name} entered MeleeChaseState");
            enemy.agent.isStopped = false; // Ensures movement resumes if stopped previously
            enemy.agent.speed = enemy.chaseSpeed;
            enemy.animator.SetBool("isChasing", true);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (enemy.target == null)
                return;

            float distance = Vector3.Distance(enemy.transform.position, enemy.target.position);

            if (distance <= enemy.attackRange)
            {
                enemy.agent.isStopped = true; // Stop moving before attacking
                stateMachine.ChangeState(enemy.AttackState);
                return;
            }

            // // Rotate towards the player smoothly
            // Quaternion targetRotation = Quaternion.LookRotation(
            //     enemy.target.position - enemy.transform.position
            // );
            // enemy.transform.rotation = Quaternion.Slerp(
            //     enemy.transform.rotation,
            //     targetRotation,
            //     Time.deltaTime * 5f
            // );
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            enemy.agent.SetDestination(enemy.target.position);
        }

        public override void Exit()
        {
            base.Exit();
            enemy.animator.SetBool("isChasing", false);
        }
    }
}
