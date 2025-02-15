using EnemyAI.Base;
using UnityEngine;

namespace EnemyAI.States.Melee
{
    public class MeleeAttackState : State
    {
        private float attackCooldown = 1.5f;
        private float lastAttackTime;
        private bool isAttacking = false;

        public MeleeAttackState(EnemyBase _enemy, StateMachine _stateMachine)
            : base(_enemy, _stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            isAttacking = true;
            Debug.Log($"{enemy.name} entered MeleeAttackState");
            PerformAttack();
        }

        public void PerformAttack()
        {
            if (!isAttacking)
                return; // Prevent unintended calls

            Debug.Log($"{enemy.name} is attacking!");
            if (enemy.target != null)
            {
                // Instantly face the player when attacking
                Vector3 direction = (enemy.target.position - enemy.transform.position).normalized;
                direction.y = 0; // Prevent tilting up/down
                enemy.transform.rotation = Quaternion.LookRotation(direction);
            }
            enemy.animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Check if attack should restart
            if (Time.time - lastAttackTime >= attackCooldown && isAttacking)
            {
                Debug.Log($"{enemy.name} cooldown over, attacking again!");
                PerformAttack(); // Restart attack
            }

            // Stop attacking if player moves away
            if (
                Vector3.Distance(enemy.transform.position, enemy.target.position)
                > enemy.attackRange
            )
            {
                Debug.Log($"{enemy.name} target out of range, switching to ChaseState!");
                isAttacking = false;
                stateMachine.ChangeState(enemy.ChaseState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            isAttacking = false;
            Debug.Log($"{enemy.name} exited MeleeAttackState");
        }
    }
}
