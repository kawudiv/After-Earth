using EnemyAI.Base;
using EnemyAI.States.Boss;
using UnityEngine;

namespace EnemyAI.Enemies.Boss
{
    public class BossEnemy : EnemyBase
    {
        private BossChaseState bossChaseState;
        private BossAttackState bossAttackState;
        private BossSpecialAttackState bossSpecialAttackState;

        public override State ChaseState => bossChaseState;
        public override State AttackState =>
            CanUseSpecialAttack ? bossSpecialAttackState : bossAttackState;

        protected override void Awake()
        {
            base.Awake();

            // Initialize states
            bossChaseState = new BossChaseState(this, stateMachine); // Changed movementSM -> stateMachine
            bossAttackState = new BossAttackState(this, stateMachine);
            bossSpecialAttackState = new BossSpecialAttackState(this, stateMachine);

            stateMachine.Initialize(bossChaseState); // Changed movementSM -> stateMachine
        }

        /// <summary>
        /// Determines if the boss can use the special attack (if health is below 30%)
        /// </summary>
        public bool CanUseSpecialAttack => health <= maxHealth * 0.3f;

        /// <summary>
        /// Executes a special attack.
        /// </summary>
        public virtual void PerformSpecialAttack()
        {
            Debug.Log($"{name} performs a powerful special attack!");
        }
    }
}
