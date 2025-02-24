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
                Debug.Log("[RunState] Rolling detected. Transitioning to RollState.");
                stateMachine.ChangeState(player.RollState);
                return;
            }

            if (moveInput == Vector2.zero)
            {
                Debug.Log("[RunState] No movement input. Transitioning to IdleState.");
                stateMachine.ChangeState(player.IdleState);
                return;
            }

            if (player.PlayerInputHandler.IsSprinting)
            {
                Debug.Log("[RunState] Sprinting input active. Transitioning to SprintState.");
                stateMachine.ChangeState(player.SprintState);
                return;
            }

            if (player.PlayerInputHandler.IsWalking)
            {
                Debug.Log("[RunState] Walk toggle active. Transitioning to WalkState.");
                stateMachine.ChangeState(player.WalkState);
                return;
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            ApplyGravity();

            // ✅ Smooth acceleration
            MoveCharacter(player.PlayerInputHandler.MoveInput, player.runSpeed);
        }

        public override void Exit()
        {
            base.Exit();
#if UNITY_EDITOR
            Debug.Log("[State] Exiting Run");
#endif
        }
    }
}
