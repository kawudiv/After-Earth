using EnemyAI.Base;
using EnemyAI.States.Common;
using EnemyAI.States.Melee;
using UnityEngine;

namespace EnemyAI.Enemies.Melee
{
    public class MeleeEnemy : EnemyBase
    {
        private MeleeChaseState _meleeChaseState;
        private MeleeAttackState _meleeAttackState;
        private FleeState _fleeState;

        public override State ChaseState => _meleeChaseState;
        public override State AttackState => _meleeAttackState;
        public override State FleeState => _fleeState;

        protected override void Awake()
        {
            base.Awake();

            // Assign melee-specific states
            _meleeChaseState = new MeleeChaseState(this, stateMachine);
            _meleeAttackState = new MeleeAttackState(this, stateMachine);
            _fleeState = new FleeState(this, stateMachine);

            if (canFlee)
            {
                _fleeState = new FleeState(this, stateMachine);
                Debug.Log($"{name} can flee. FleeState initialized.");
            }
            else
            {
                Debug.Log($"{name} cannot flee.");
            }

            // Ensure the AI starts in a valid state
            stateMachine.Initialize(_meleeChaseState);
            Debug.Log($"{name} initialized in ChaseState.");
        }

        protected override void Update()
        {
            base.Update();
            CheckHealthAndReact(); // ✅ Call this instead of adding logic directly
        }

        protected override void CheckHealthAndReact()
        {
            if (enemyHealth == null)
            {
                Debug.LogError($"{name} is missing an EnemyHealth component!");
                return;
            }

            float currentHealth = enemyHealth.CurrentHealth; // ✅ Get current health
            Debug.Log($"{name} Health Check: {currentHealth}");

            // ✅ Ensure we only switch states when necessary
            if (canFlee && _fleeState != null && currentHealth <= 20)
            {
                if (stateMachine.currentState != _fleeState) // ❌ Prevent re-entering the same state
                {
                    Debug.Log($"{name} health is low! Switching to FleeState.");
                    stateMachine.ChangeState(_fleeState);
                }
            }
            else if (canFlee && _fleeState != null && currentHealth >= 22) // ✅ Added a buffer to prevent rapid switching
            {
                if (stateMachine.currentState == _fleeState) // ✅ Only switch if currently fleeing
                {
                    Debug.Log($"{name} health is sufficient! Switching to PatrolState.");
                    stateMachine.ChangeState(PatrolState);
                }
            }
        }
    }
}
