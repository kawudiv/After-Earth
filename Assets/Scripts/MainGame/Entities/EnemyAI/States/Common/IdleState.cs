using UnityEngine;
using EnemyAI.Base;

namespace EnemyAI.States.Common
{
    public class IdleState : State
    {
        private float checkTime = 1f; // Time between checks
        private float timer = 0f;

        public IdleState(EnemyBase _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log($"{enemy.name} entered IdleState");
            Debug.Log($"{enemy.name} is idle");

            enemy.agent.isStopped = true;
            enemy.animator.SetBool("isIdle", true); // Changed from "isWalking" to "isIdle"
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            timer += Time.deltaTime;
            if (timer >= checkTime)
            {
                timer = 0f; // Reset timer

                float distance = Vector3.Distance(enemy.transform.position, enemy.target.position);

                if (distance <= enemy.detectionRange)
                {
                    stateMachine.ChangeState(enemy.ChaseState);
                }
                else
                {
                    stateMachine.ChangeState(enemy.PatrolState);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            enemy.animator.SetBool("isIdle", false);
            Debug.Log($"{enemy.name} exited idle state.");
        }
    }
}
