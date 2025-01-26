using UnityEngine;

public class AnimationState : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }

    void Update()
    {
        bool forwardPressed = Input.GetKey("w");
        bool backwardPressed = Input.GetKey("s");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");

        bool shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        bool isMoving = forwardPressed || backwardPressed || leftPressed || rightPressed;
        bool isSprinting = isMoving && shiftPressed;

        animator.SetBool("isRunning", isMoving);
        animator.SetBool("isSprinting", isSprinting);

        if (!shiftPressed)
        {
            animator.SetBool("isSprinting", false);
        }
    }
}
