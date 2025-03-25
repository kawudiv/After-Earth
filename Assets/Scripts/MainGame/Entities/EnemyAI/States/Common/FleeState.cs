using EnemyAI.Base;
using UnityEngine;

namespace EnemyAI.States.Common
{
    public class FleeState : State
    {
        private float fleeDistance = 10f;
        private float checkTime = 1f;
        private float minFleeDuration = 3f; // ✅ Prevents immediate exit
        private float fleeTimer = 0f;
        private float checkTimer = 0f;

        public FleeState(EnemyBase _enemy, StateMachine _stateMachine)
            : base(_enemy, _stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            fleeTimer = 0f; // ✅ Reset flee timer
            Debug.Log($"{enemy.name} entered FleeState");
            enemy.agent.enabled = true;
            enemy.agent.isStopped = false;
            enemy.agent.speed = enemy.ChaseSpeed * 1.2f; // Run slightly faster than chase
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            fleeTimer += Time.deltaTime;
            checkTimer += Time.deltaTime;

            if (checkTimer >= checkTime)
            {
                checkTimer = 0f; // Reset timer

                if (enemy != null && enemy.target != null)
                {
                    Vector3 fleeDirection = (
                        enemy.transform.position - enemy.target.position
                    ).normalized;
                    Vector3 fleePosition = enemy.transform.position + fleeDirection * fleeDistance;
                    enemy.agent.SetDestination(fleePosition);

                    // ✅ Ensure at least minFleeDuration seconds have passed before switching states
                    if (
                        fleeTimer >= minFleeDuration
                        && Vector3.Distance(enemy.transform.position, enemy.target.position)
                            >= fleeDistance
                    )
                    {
                        Debug.Log($"{enemy.name} has fled far enough. Switching to PatrolState.");
                        stateMachine.ChangeState(enemy.PatrolState);
                        return;
                    }
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log($"{enemy.name} exited FleeState");
        }
    }
}
