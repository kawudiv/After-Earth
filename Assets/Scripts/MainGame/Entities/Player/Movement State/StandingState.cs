using UnityEngine;
namespace Player.Movement

 {   
    public class StandingState: State
    {
        
        float gravityValue;
        bool jump;   
        bool crouch;
        Vector3 currentVelocity;
        bool grounded;
        bool sprint;
        float playerSpeed;
    
        Vector3 cVelocity;
    
        public StandingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
        {
            character = _character;
            stateMachine = _stateMachine;
        }
    
        public override void Enter()
        {
            base.Enter();
    
            jump = false;
            crouch = false;
            sprint = false;
            input = Vector2.zero;
            velocity = Vector3.zero;
            currentVelocity = Vector3.zero;
            gravityVelocity.y = 0;
    
            playerSpeed = character.playerSpeed;
            grounded = character.controller.isGrounded;
            gravityValue = character.gravityValue;    
        }
    
        public override void HandleInput()
        {
            base.HandleInput();
    
            if (jumpAction.triggered)
            {
                jump = true;
            }
            if (crouchAction.triggered)
            {
                crouch = true;
            }
            if (sprintAction.triggered)
            {
                sprint = true;
            }
    
            input = moveAction.ReadValue<Vector2>();
            velocity = new Vector3(input.x, 0, input.y);
    
            velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
            velocity.y = 0f;
        
        }
    
        public override void LogicUpdate()
        {
            base.LogicUpdate();
    
            character.animator.SetFloat("speed", input.magnitude, character.speedDampTime, Time.deltaTime);
    
            if (sprint)
            {
                stateMachine.ChangeState(character.sprinting);
            }    
            if (jump)
            {
                stateMachine.ChangeState(character.jumping);
            }
            if (crouch)
            {
                stateMachine.ChangeState(character.crouching);
            }
            
        }
    
        public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        Debug.Log($"Grounded: {grounded}");
        Debug.Log($"Gravity Velocity (Before Reset): {gravityVelocity.y}");

        if (grounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0f;
            Debug.Log("Gravity velocity reset due to being grounded.");
        }

        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity, ref cVelocity, character.velocityDampTime);
        
        Debug.Log($"Current Velocity: {currentVelocity}");
        Debug.Log($"Velocity: {velocity}");
        Debug.Log($"Character Speed: {playerSpeed}");

        character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);

        if (velocity.sqrMagnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, targetRotation, character.rotationDampTime);
            
            Debug.Log($"Character Rotation: {character.transform.rotation.eulerAngles}");
        }
    }
    
        public override void Exit()
        {
            base.Exit();
    
            gravityVelocity.y = 0f;
            character.playerVelocity = new Vector3(input.x, 0, input.y);
    
            if (velocity.sqrMagnitude > 0)
            {
                character.transform.rotation = Quaternion.LookRotation(velocity);
            }
        }
    
    }
}