using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Player.Components
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField]
        private PlayerInventory playerInventory;

        [SerializeField]
        private PlayerCombat playerCombat;

        public Vector2 MoveInput { get; private set; }
        public bool IsSprinting { get; set; }
        public bool IsRolling { get; set; }
        public bool IsWalking { get; private set; } = false;
        public bool IsDraw { get; set; }
        public bool IsBlock { get; set; }

        // New flags for differentiating draw input.
        public bool IsMeleeDraw { get; private set; }
        public bool IsRangedDraw { get; private set; }

        // Attack
        public bool IsAttack { get; private set; }

        // Movement Toggle
        public bool CanMove { get; private set; } = true;

        private Dictionary<string, bool> values = new Dictionary<string, bool>();

        private void OnMove(InputValue value)
        {
            if (!CanMove)
            {
                MoveInput = Vector2.zero; // Prevent movement input
                Debug.Log("🚫 [PlayerInputHandler] Movement is disabled!");
                return;
            }
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

        // New OnDraw method to distinguish between melee (1) and ranged (2) draw inputs.
        private void OnDraw(InputValue value)
        {
            // Assume the input action is set up to send a float value:
            // 1 for Melee and 2 for Ranged. (You may need to adjust your Input Action setup accordingly.)
            float drawInput = value.Get<float>();

            // Reset both flags initially.
            ClearDraw();

            if (drawInput == 1f)
            {
                IsMeleeDraw = true;
                IsDraw = true;
                Debug.Log($"[PlayerInputHandler] Melee weapon draw triggered.");
            }
            else if (drawInput == 2f)
            {
                IsRangedDraw = true;
                IsDraw = true;
                Debug.Log($"[PlayerInputHandler] Ranged weapon draw triggered.");
            }
            else
            {
                Debug.Log($"[PlayerInputHandler] Draw released or unrecognized value: {drawInput}");
            }
        }

        public void ClearDraw()
        {
            IsMeleeDraw = false;
            IsRangedDraw = false;
            IsDraw = false;
            Debug.Log("[PlayerInputHandler] Melee draw flag cleared.");
        }

        public void OnAttack()
        {
            if (!playerCombat.IsEquip())
            {
                Debug.Log("[PlayerInputHandler] Cannot attack: No melee weapon equipped.");
                return;
            }

            if (playerCombat.IsAttacking())
            {
                Debug.Log("[PlayerInputHandler] Cannot attack: Already attacking.");
                return;
            }

            Debug.Log("[PlayerInputHandler] Light Attack (Tap).");
            IsAttack = true;
        }

        private void OnPickUpWeapon(InputValue value)
        {
            if (value.isPressed)
            {
                if (playerInventory == null)
                {
                    Debug.LogError("[PlayerInputHandler] PlayerInventory is not assigned!");
                    return;
                }

                Debug.Log("Trying to pick up weapon...");
                playerInventory.TryPickUpPrefab();
            }
        }

        private void OnDropWeapon(InputValue value)
        {
            if (value.isPressed)
            {
                if (playerInventory != null)
                {
                    Debug.Log("Dropping current weapon...");
                    playerInventory.DropCurrentWeapon();
                }
                else
                {
                    Debug.LogError("PlayerInventory is not assigned in PlayerInputHandler!");
                }
            }
        }

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

        public void SetAllDraw(bool value)
        {
            IsMeleeDraw = value;
            IsMeleeDraw = value;
            IsDraw = value;
        }

        public void SetIsAttack(bool value)
        {
            IsAttack = value;
        }

        public void SetCanMove(bool value)
        {
            CanMove = value;
        }

        public void SetValue(string key, bool value)
        {
            values[key] = value;
        }
    }
}
