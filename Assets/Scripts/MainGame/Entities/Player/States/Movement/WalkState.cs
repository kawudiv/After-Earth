using Player.Base;
using UnityEngine;

namespace Player.States.Movement
{
    public class WalkState : BaseMovementState
    {
        public WalkState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("[State] Entered Walk");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            Vector2 moveInput = player.PlayerInputHandler.MoveInput;

            // Check for roll input first.
            if (player.PlayerInputHandler.IsRolling && characterController.isGrounded)
            {
                Debug.Log("[WalkState] Rolling detected. Transitioning to RollState.");
                stateMachine.ChangeState(player.RollState);
                return;
            }

            // If there's no movement input, transition to Idle.
            if (moveInput == Vector2.zero)
            {
                Debug.Log("[WalkState] No movement input. Transitioning to IdleState.");
                stateMachine.ChangeState(player.IdleState);
                return;
            }

            // If the sprint key is active, transition to SprintState.
            if (player.PlayerInputHandler.IsSprinting)
            {
                Debug.Log("[WalkState] Sprint input active. Transitioning to SprintState.");
                stateMachine.ChangeState(player.SprintState);
                return;
            }

            // If the walk toggle is deactivated (i.e., the player wants to run), transition to RunState.
            if (!player.PlayerInputHandler.IsWalking)
            {
                Debug.Log("[WalkState] Walk toggle is off. Transitioning to RunState.");
                stateMachine.ChangeState(player.RunState);
                return;
            }

            // If none of the above transitions occur, remain in WalkState.
            Debug.Log("[WalkState] Continuing WalkState.");
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            ApplyGravity();

            // Use the walk speed (2) for movement.
            MoveCharacter(player.PlayerInputHandler.MoveInput, player.walkSpeed);
        }

        public override void Exit()
        {
            base.Exit();
#if UNITY_EDITOR
            Debug.Log("[State] Exiting Walk");
#endif
        }
    }
}
