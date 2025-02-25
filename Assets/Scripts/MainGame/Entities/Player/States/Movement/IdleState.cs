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
            Vector2 moveInput = player.PlayerInputHandler.MoveInput;

            if (player.PlayerInputHandler.IsRolling && characterController.isGrounded)
            {
                Debug.Log("[IdleState] Rolling detected. Transitioning to RollState.");
                stateMachine.ChangeState(player.RollState);
                return;
            }

            if (moveInput == Vector2.zero)
            {
                Debug.Log("[IdleState] No movement input. Remaining Idle.");
                return;
            }

            // Check the toggle to decide between Walk and Run:
            if (player.PlayerInputHandler.IsWalking)
            {
                Debug.Log("[IdleState] Walk toggle active. Transitioning to WalkState.");
                stateMachine.ChangeState(player.WalkState);
            }
            else
            {
                Debug.Log("[IdleState] Walk toggle inactive. Transitioning to RunState.");
                stateMachine.ChangeState(player.RunState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            ApplyGravity();

            // Gradually decelerate when no input is provided.
            MoveCharacter(Vector2.zero, 0f);
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("[State] Exiting Idle");
        }
    }
}
