using UnityEngine;
using EnemyAI.Base;

namespace EnemyAI.States.Common
{
    public class PatrolState : State
    {
        private Vector3 patrolCenter;
        private float patrolRadius = 10f;
        private int maxRetries = 5;
        private Vector3 currentPatrolPoint;

        public PatrolState(EnemyBase _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) 
        { 
            patrolCenter = _enemy.transform.position;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log($"{enemy.name} entered PatrolState");

            enemy.agent.speed = enemy.patrolSpeed;
            enemy.agent.isStopped = false;
            enemy.animator.SetBool("isWalking", true);

            SetRandomPatrolPoint();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Vector3.Distance(enemy.transform.position, enemy.target.position) <= enemy.detectionRange)
            {
                stateMachine.ChangeState(enemy.ChaseState);
                return;
            }

            if (!enemy.agent.pathPending && enemy.agent.remainingDistance < 0.5f)
            {
                SetRandomPatrolPoint();
            }

            // Debugging
            DebugDrawPatrolArea();
            DebugDrawPath();
        }

        public override void Exit()
        {
            base.Exit();
            enemy.animator.SetBool("isWalking", false);
            Debug.Log($"{enemy.name} stopped patrolling.");
        }

        private void SetRandomPatrolPoint()
        {
            Vector3 randomPoint = Vector3.zero;
            bool validPointFound = false;

            for (int i = 0; i < maxRetries; i++)
            {
                Vector3 randomOffset = Random.insideUnitSphere * patrolRadius;
                randomOffset.y = 0;
                randomPoint = patrolCenter + randomOffset;

                UnityEngine.AI.NavMeshHit hit;
                if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
                {
                    randomPoint = hit.position;
                    validPointFound = true;
                    break;
                }
            }

            if (validPointFound)
            {
                currentPatrolPoint = randomPoint; // Store for visualization
                enemy.agent.SetDestination(randomPoint);
            }
            else
            {
                Debug.LogWarning($"{enemy.name}: No valid patrol point found.");
            }
        }

        /// <summary>
        /// Draws a debug visualization of the patrol area in the Scene view.
        /// </summary>
        private void DebugDrawPatrolArea()
        {
            const int segments = 36; // Number of segments to draw the circle
            float anglePerSegment = 360f / segments;

            for (int i = 0; i < segments; i++)
            {
                Vector3 startPoint = patrolCenter + Quaternion.Euler(0, anglePerSegment * i, 0) * Vector3.forward * patrolRadius;
                Vector3 endPoint = patrolCenter + Quaternion.Euler(0, anglePerSegment * (i + 1), 0) * Vector3.forward * patrolRadius;
                Debug.DrawLine(startPoint, endPoint, Color.yellow);
            }
        }

        /// <summary>
        /// Draws the current path of the NavMeshAgent.
        /// </summary>
        private void DebugDrawPath()
        {
            if (enemy.agent.hasPath)
            {
                Vector3[] pathCorners = enemy.agent.path.corners;
                for (int i = 0; i < pathCorners.Length - 1; i++)
                {
                    Debug.DrawLine(pathCorners[i], pathCorners[i + 1], Color.blue);
                }
            }
        }
    }
}
