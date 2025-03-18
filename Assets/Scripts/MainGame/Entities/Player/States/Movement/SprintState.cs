using Player.Base;
using UnityEngine;

namespace Player.States.Movement
{
    public class SprintState : BaseMovementState
    {
        public SprintState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Entered Sprint State");
            speedTransitionTime = 0f;
            CanMove = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!CanMove)
            {
                Debug.Log("[SprintState] CanMove is FALSE. Player remains idle.");
                return; // Prevent movement state transitions
            }

            Vector2 moveInput = player.PlayerInputHandler.MoveInput;
            bool isSprinting = player.PlayerInputHandler.IsSprinting;

            if (player.PlayerInputHandler.IsRolling && characterController.isGrounded)
            {
                Debug.Log("🔄 Transitioning to RollState");
                stateMachine.ChangeState(player.RollState);
                return;
            }

            if (moveInput == Vector2.zero)
            {
                Debug.Log("🏁 Transitioning Sprint → IdleState (No movement input)");
                player.PlayerInputHandler.IsSprinting = false; // Ensure it's reset
                stateMachine.ChangeState(player.IdleState);
                return;
            }

            if (!isSprinting) // ✅ Proper transition from Sprint → Run
            {
                Debug.Log("🏃 Transitioning Sprint → RunState (Shift released)");
                stateMachine.ChangeState(player.RunState);
                return;
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            ApplyGravity();
            MoveCharacter(player.PlayerInputHandler.MoveInput, player.sprintSpeed);
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Exiting Sprint State");
        }
    }
}
