using EnemyAI.Base;
using EnemyAI.States.Melee;

namespace EnemyAI.Enemies.Melee
{
    public class MeleeEnemy : EnemyBase
    {
        private MeleeChaseState _meleeChaseState;
        private MeleeAttackState _meleeAttackState;

        public override State ChaseState => _meleeChaseState;
        public override State AttackState => _meleeAttackState;

        protected override void Awake()
        {
            base.Awake();

            // Assign melee-specific states
            _meleeChaseState = new MeleeChaseState(this, stateMachine);
            _meleeAttackState = new MeleeAttackState(this, stateMachine);

            // Ensure the AI starts in a valid state
            stateMachine.Initialize(_meleeChaseState);
        }
    }
}
