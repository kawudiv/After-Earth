using UnityEngine;
namespace Player.Movement
{
    public class JumpingState : State
    {
        bool grounded;
        float gravityValue;
        float jumpHeight;
        float playerSpeed;
        Vector3 airVelocity;
        private bool jumpStarted = false; // Track if jump has started

        public JumpingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
        {
            character = _character;
            stateMachine = _stateMachine;
        }

        public override void Enter()
        {
            base.Enter();

            grounded = false;
            gravityValue = character.gravityValue;
            jumpHeight = character.jumpHeight;
            playerSpeed = character.playerSpeed;
            gravityVelocity.y = 0;

            character.animator.SetFloat("speed", 0);
            character.animator.SetTrigger("jump");

            // Ensure the jump isn't applied right away
            jumpStarted = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Get current state info from animator
            AnimatorStateInfo stateInfo = character.animator.GetCurrentAnimatorStateInfo(0);

            // Check if we're in the "Jump" animation and if it's at a certain point in the animation
            if (stateInfo.IsName("Jump") && stateInfo.normalizedTime >= 0.2f && !jumpStarted)
            {
                Jump();
                jumpStarted = true; // Ensure we don't apply the jump force multiple times
            }

            if (grounded)
            {
                stateMachine.ChangeState(character.landing);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!grounded)
            {
                velocity = character.playerVelocity;
                airVelocity = new Vector3(input.x, 0, input.y);

                velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
                velocity.y = 0f;
                airVelocity = airVelocity.x * character.cameraTransform.right.normalized + airVelocity.z * character.cameraTransform.forward.normalized;
                airVelocity.y = 0f;

                character.controller.Move(
                    gravityVelocity * Time.deltaTime +
                    (airVelocity * character.airControl + velocity * (1 - character.airControl)) * playerSpeed * Time.deltaTime
                );
            }

            gravityVelocity.y += gravityValue * Time.deltaTime;
            grounded = character.controller.isGrounded;
        }

        void Jump()
        {
            gravityVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }
}