using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool IsSprinting { get; set; } // ✅ Allow setting from outside
    public bool IsRolling { get; set; }

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

    public void ResetRoll() // ✅ Called after roll completes
    {
        IsRolling = false;
        Debug.Log("[PlayerInputHandler] Roll reset, ready to roll again");
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
