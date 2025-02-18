using Player.Base;
using UnityEngine;

namespace Player.States.Movement
{
    public class IdleState : BaseMovementState
    {
        public IdleState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("[State] Entered Idle");
            speedTransitionTime = 0f; // Reset transition time
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (player.PlayerInputHandler.IsRolling && characterController.isGrounded)
            {
                Debug.Log("🔄 Transitioning from Sprint → RollState");
                stateMachine.ChangeState(player.RollState);
                return;
            }

            if (player.PlayerInputHandler.MoveInput != Vector2.zero)
            {
                stateMachine.ChangeState(player.RunState);
                return;
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            ApplyGravity();

            // ✅ Gradually stop instead of instant halt
            MoveCharacter(Vector2.zero, 0f);

            // ✅ Sync animation smoothly
            player.PlayerAnimation.SetMovementSpeed(0f);
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("[State] Exiting Idle");
        }
    }
}
