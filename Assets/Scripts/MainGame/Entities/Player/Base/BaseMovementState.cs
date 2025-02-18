using Core;
using Player.Components;
using UnityEngine;

namespace Player.Base
{
    public abstract class BaseMovementState : State
    {
        protected readonly CharacterController characterController;
        protected readonly PlayerAnimation playerAnimation;
        protected Vector3 velocity;

        private float currentSpeed = 0f;
        private float speedTransitionDuration = 0.3f;
        public float speedTransitionTime = 0f;

        protected BaseMovementState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine)
        {
            characterController = player.characterController;
            playerAnimation = player.PlayerAnimation;
        }

        // Move the character with smooth transitions
        protected void MoveCharacter(Vector2 moveInput, float targetSpeed)
        {
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

            // Rotate only if there's movement input
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                characterController.transform.rotation = Quaternion.Slerp(
                    characterController.transform.rotation,
                    targetRotation,
                    Time.deltaTime * player.rotationSpeed
                );
            }

            // Smooth speed transition
            currentSpeed = Mathf.Lerp(
                currentSpeed,
                targetSpeed,
                Time.deltaTime / speedTransitionDuration
            );

            // Apply movement
            characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
        }

        // Gradually slow down when no input is given
        protected void Decelerate()
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime / speedTransitionDuration);
            characterController.Move(Vector3.forward * currentSpeed * Time.deltaTime);
        }

        public void PerformRoll()
        {
            if (player.PlayerInputHandler.IsRolling && characterController.isGrounded)
            {
                Debug.Log("🔄 Rolling initiated!");
                stateMachine.ChangeState(player.RollState);
            }
        }

        // Apply gravity
        protected void ApplyGravity()
        {
            if (!characterController.isGrounded)
            {
                velocity.y += GlobalConstants.Gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0f; // Reset gravity when grounded
            }

            characterController.Move(velocity * Time.deltaTime);
        }
    }
}
