using EnemyAI.Base;
using EnemyAI.States.Melee;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI.Components
{
    public class EnemyAnimation : MonoBehaviour
    {
        private Animator animator;
        private NavMeshAgent agent;
        private EnemyBase enemy; // Reference to EnemyBase

        private void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            enemy = GetComponent<EnemyBase>(); // Get the EnemyBase component
        }

        private void Update()
        {
            float speed = agent.velocity.magnitude; // Get the actual movement speed
            animator.SetFloat("Speed", speed); // Update Blend Tree parameter
            animator.SetFloat("Enemy", enemy.enemyID);
        }

        public void SetTrigger(string parameter)
        {
            animator.SetTrigger(parameter);
        }

        public void PlayHitReaction()
        {
            animator.SetTrigger("Hit");
        }

        public void PlayDeath()
        {
            animator.SetTrigger("Death");
        }

        public void OnAttackAnimationEnd()
{
    if (TryGetComponent(out EnemyBase enemy))
    {
        if (enemy.target != null)
        {
            float distance = Vector3.Distance(enemy.transform.position, enemy.target.position);
            Debug.Log($"{enemy.name} Distance to Target After Attack: {distance}, Attack Range: {enemy.attackRange}");

            if (distance <= enemy.attackRange)
            {
                Debug.Log($"{enemy.name} still in attack range, re-entering AttackState.");
                enemy.stateMachine.ChangeState(enemy.AttackState); // Continue attacking
            }
            else if (distance <= enemy.detectionRange)
            {
                Debug.Log($"{enemy.name} lost attack range but still detects player. Returning to ChaseState.");
                enemy.agent.isStopped = false; // Resume movement
                enemy.stateMachine.ChangeState(enemy.ChaseState); // Resume chasing
            }
            else
            {
                Debug.Log($"{enemy.name} lost detection range. Returning to PatrolState.");
                enemy.stateMachine.ChangeState(enemy.PatrolState); // Go back to patrol if player is gone
            }
        }
        else
        {
            Debug.LogWarning($"{enemy.name} has no target! Returning to PatrolState.");
            enemy.stateMachine.ChangeState(enemy.PatrolState);
        }
    }
}

    }
}
