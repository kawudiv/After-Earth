using Player.Base;
using UnityEngine;

namespace Player.States.Combat.Melee
{
    public class AttackMeleeState : ToggleMeleeWeaponState
    {
        private float attackDuration;
        private const float defaultAttackDuration = 0.7f; // Adjust as needed

        public AttackMeleeState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("[AttackMeleeState] Entered AttackMeleeState");
            // Trigger the attack animation.
            player.PlayerAnimation.SetTrigger("MeleeAttack");
            attackDuration = defaultAttackDuration;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            attackDuration -= Time.deltaTime;
            // You might call the weapon's Attack() method at the appropriate moment via an Animation Event.
            if (attackDuration <= 0f)
            {
                Debug.Log("[AttackMeleeState] Attack complete, transitioning to CombatIdleState");
                // stateMachine.ChangeState(player.CombatIdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("[AttackMeleeState] Exiting AttackMeleeState");
        }

        // Optionally, you can have a method to perform the attack (triggered by an Animation Event).
        public void PerformAttack()
        {
            if (equippedWeapon != null)
            {
                equippedWeapon.Attack();
            }
        }
    }
}
