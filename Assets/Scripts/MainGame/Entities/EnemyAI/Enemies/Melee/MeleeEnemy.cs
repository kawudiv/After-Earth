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

            if (canFlee)
            {
                _fleeState = new FleeState(this, stateMachine);
                Debug.Log($"{name} can flee. FleeState initialized.");
            }
            else
            {
                _fleeState = null; // Ensure no invalid state reference
                Debug.Log($"{name} cannot flee.");
            }

            // Ensure the AI starts in a valid state
            stateMachine.Initialize(_meleeChaseState);
            Debug.Log($"{name} initialized in ChaseState.");
        }

        protected override void Update()
        {
            base.Update();
            CanFlee(); // ✅ No need for parameters, `canFlee` will be checked inside
        }

        public void CanFlee()
        {
            if (!canFlee || _fleeState == null || enemyHealth == null)
                return; // ✅ Skip fleeing behavior if `canFlee` is false

            float currentHealth = enemyHealth.CurrentHealth;
            float maxHealth = enemyHealth.MaxHealth; // Assuming MaxHealth is available
            float healthPercentage = currentHealth / maxHealth * 100f;

            Debug.Log($"{name} Health Check for Fleeing: {currentHealth} ({healthPercentage}%)");

            if (healthPercentage <= 50 && stateMachine.currentState != _fleeState)
            {
                Debug.Log($"{name} health is below 50%! Switching to FleeState.");
                stateMachine.ChangeState(_fleeState);
            }
            else if (healthPercentage >= 60 && stateMachine.currentState == _fleeState)
            {
                Debug.Log($"{name} health recovered to 60%! Returning to PatrolState.");
                stateMachine.ChangeState(PatrolState);
            }
        }
    }
}
