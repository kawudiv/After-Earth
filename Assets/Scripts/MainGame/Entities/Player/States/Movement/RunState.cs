using Player.Base;
using UnityEngine;

namespace Player.States.Movement
{
    public class RunState : BaseMovementState
    {
        public RunState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("[State] Entered Run");
            speedTransitionTime = 0f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Vector2 moveInput = player.PlayerInputHandler.MoveInput;

            if (player.PlayerInputHandler.IsRolling && characterController.isGrounded)
            {
                Debug.Log("🔄 Transitioning to RollState");
                stateMachine.ChangeState(player.RollState);
                return;
            }

            if (moveInput == Vector2.zero)
            {
                stateMachine.ChangeState(player.IdleState);
                return;
            }

            if (player.PlayerInputHandler.IsSprinting)
            {
                stateMachine.ChangeState(player.SprintState);
                return;
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            ApplyGravity();

            Vector2 moveInput = player.PlayerInputHandler.MoveInput;

            // ✅ Smooth acceleration
            MoveCharacter(moveInput, player.runSpeed);

            // ✅ Sync animation with movement speed
            player.PlayerAnimation.SetMovementSpeed(player.runSpeed);
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("[State] Exiting Run");
        }
    }
}
