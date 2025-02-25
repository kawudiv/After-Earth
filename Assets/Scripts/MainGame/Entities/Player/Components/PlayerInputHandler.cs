using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool IsSprinting { get; set; }
    public bool IsRolling { get; set; }
    public bool IsWalking { get; private set; } = false;
    public bool IsDraw { get; set; }
    public bool IsBlock { get; set; }

    private void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();

        // ✅ Fix: If input magnitude is too small, set to zero to prevent drifting
        if (MoveInput.magnitude < 0.1f)
        {
            MoveInput = Vector2.zero;
        }

        // 🔍 Debugging: Log movement updates
        Debug.Log(
            $"📌 [PlayerInputHandler] MoveInput: {MoveInput} (Magnitude: {MoveInput.magnitude})",
            this
        );
    }

    private void OnSprint(InputValue value)
    {
        IsSprinting = value.isPressed;

        // 🔍 Debugging: Log sprint state
        Debug.Log($"🏃 [PlayerInputHandler] IsSprinting: {IsSprinting}", this);
    }

    private void OnRoll(InputValue value)
    {
        if (value.isPressed)
        {
            IsRolling = true;
            Debug.Log($"[PlayerInputHandler] IsRolling: {IsRolling}");
        }
    }

    private void OnToggle(InputValue value)
    {
        Debug.Log($"[PlayerInputHandler] OnToggleWalk called. Value: {value}");
        if (value.isPressed)
        {
            IsWalking = !IsWalking;
            Debug.Log($"[PlayerInputHandler] Toggle Walk: {IsWalking}", this);
        }
        else
        {
            Debug.Log($"[PlayerInputHandler] OnToggleWalk: Key released.", this);
        }
    }

    public void ResetRoll() // ✅ Called after roll completes
    {
        IsRolling = false;
        Debug.Log("[PlayerInputHandler] Roll reset, ready to roll again");
    }

    private void OnDraw(InputValue value)
    {
        IsDraw = value.isPressed;
        Debug.Log($"[PlayerInputHandler] IsDraw: {IsDraw}");
    }

    // public void OnAttack(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
    //         if (context.interaction is TapInteraction)
    //         {
    //             Debug.Log("Light Attack (Tap)");
    //         }
    //         else if (context.interaction is HoldInteraction)
    //         {
    //             Debug.Log("Heavy Attack (Hold)");
    //         }
    //     }
    // }

    private void OnBlock(InputValue value)
    {
        IsBlock = value.isPressed;
        Debug.Log($"[PlayerInputHandler] IsBlock: {IsBlock}");
    }

    private void Update()
    {
        // ✅ Extra debug check in case inputs don't update correctly
        if (MoveInput == Vector2.zero && IsSprinting)
        {
            Debug.LogWarning(
                "⚠️ Sprinting while MoveInput is zero. This might be unintended!",
                this
            );
        }
    }
}
