using Player.Base;
using UnityEngine;

namespace Player.States.Movement
{
    public class RollState : BaseMovementState
    {
        private float rollTime; // Duration of the roll
        private Vector3 rollDirection; // Direction of the roll

        public RollState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("🎭 Entered Roll State");

            // ✅ Reset IsRolling to prevent multiple rolls
            player.PlayerInputHandler.IsRolling = false;

            rollTime = player.rollDuration;

            // ✅ Use movement input for precise rolling
            Vector2 moveInput = player.PlayerInputHandler.MoveInput;

            if (moveInput.magnitude > 0.1f) // Ensure input is meaningful
            {
                rollDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
                player.transform.forward = rollDirection; // ✅ Rotate player to roll direction
            }
            else
            {
                rollDirection = player.transform.forward; // Default to forward roll
            }

            playerAnimation.SetTrigger("RollTrigger");
            player.PlayerHealth.IsInvulnerable = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Decrease roll timer
            rollTime -= Time.deltaTime;

            // Transition to RunState after roll ends
            if (rollTime <= 0)
            {
                Debug.Log("✅ Transitioning from RollState to RunState");
                stateMachine.ChangeState(player.RunState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            // Move the player in the roll direction
            characterController.Move(rollDirection * player.rollSpeed * Time.deltaTime);

            // Apply gravity only if not grounded
            if (!characterController.isGrounded)
            {
                ApplyGravity();
            }
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("🚪 Exiting Roll State");

            // Reset animation trigger
            playerAnimation.ResetTrigger("RollTrigger");

            // Remove invulnerability after roll
            player.PlayerHealth.IsInvulnerable = false;

            // ✅ Reset roll flag so player can roll again after finishing
            player.PlayerInputHandler.ResetRoll();
        }
    }
}
