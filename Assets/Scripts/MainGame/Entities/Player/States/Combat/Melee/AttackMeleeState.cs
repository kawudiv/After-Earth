using Player.Base;
using UnityEngine;

namespace Player.States.Combat.Melee
{
    public class AttackMeleeState : BaseMeleeState
    {
        private bool attackCompleted;

        public AttackMeleeState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("[AttackMeleeState] Entered AttackMeleeState");

            // Trigger melee attack animation
            player.PlayerInputHandler.SetCanMove(false);
            player.PlayerAnimation.SetTrigger("MeleeAttack");

            Debug.Log("[AttackMeleeState] Triggered MeleeAttack animation.");

            // Reset attack completion flag
            attackCompleted = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            Debug.Log("[AttackMeleeState] LogicUpdate running...");

            // If attack animation has finished, transition back to Idle
            if (attackCompleted)
            {
                Debug.Log(
                    "[AttackMeleeState] Attack animation completed. Transitioning to IdleState."
                );
                stateMachine.ChangeState(player.IdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("[AttackMeleeState] Exiting AttackMeleeState");

            // Reset attack flag in PlayerInputHandler
            player.PlayerInputHandler.SetIsAttack(false);
            player.PlayerInputHandler.SetCanMove(true);
            Debug.Log("[AttackMeleeState] SetIsAttack(false) called on PlayerInputHandler.");

        }

        /// <summary>
        /// Call this method from an animation event when the attack animation is complete.
        /// </summary>
        public void OnAttackComplete()
        {
            Debug.Log("[AttackMeleeState] OnAttackComplete() called via animation event.");
            attackCompleted = true;
        }
    }
}
