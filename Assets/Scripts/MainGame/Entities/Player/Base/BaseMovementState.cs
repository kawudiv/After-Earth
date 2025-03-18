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
        private float speedTransitionDuration = 0.1f;
        public float speedTransitionTime = 0f;

        public bool CanMove { get; set; } = true; // Movement toggle

        protected BaseMovementState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine)
        {
            characterController = player.characterController;
            playerAnimation = player.PlayerAnimation;
        }

        protected void MoveCharacter(Vector2 moveInput, float targetSpeed)
        {
            if (!CanMove)
                return; // Prevent movement when disabled

            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                characterController.transform.rotation = Quaternion.Slerp(
                    characterController.transform.rotation,
                    targetRotation,
                    Time.deltaTime * player.rotationSpeed
                );
            }

            currentSpeed = Mathf.Lerp(
                currentSpeed,
                targetSpeed,
                Time.deltaTime / speedTransitionDuration
            );
            characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
            playerAnimation.UpdateSpeed(currentSpeed);
        }

        protected void Decelerate()
        {
            if (!CanMove)
                return; // Prevent deceleration updates

            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime / speedTransitionDuration);
            playerAnimation.UpdateSpeed(currentSpeed);
        }

        public void PerformRoll()
        {
            if (!CanMove)
                return; // Prevent rolling when movement is disabled

            if (player.PlayerInputHandler.IsRolling && characterController.isGrounded)
            {
                Debug.Log("🔄 Rolling initiated!");
                stateMachine.ChangeState(player.RollState);
            }
        }

        protected void ApplyGravity()
        {
            if (!characterController.isGrounded)
            {
                velocity.y += GlobalConstants.Gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0f;
            }

            characterController.Move(velocity * Time.deltaTime);
        }
    }
}
