using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Weapons.Types;

namespace Player.Components
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField]
        private PlayerInventory playerInventory;

        [SerializeField]
        private PlayerCombat playerCombat;

        public Vector2 MoveInput { get; private set; }
        public Vector3 LookTarget { get; private set; }
        public bool IsSprinting { get; set; }
        public bool IsRolling { get; set; }
        public bool IsWalking { get; private set; } = false;
        public bool IsDraw { get; set; }
        public bool IsBlock { get; set; }
        public bool IsAim { get; set; }

        // New flags for differentiating draw input.
        public bool IsMeleeDraw { get; private set; }
        public bool IsRangedDraw { get; private set; }

        // Attack
        public bool IsAttack { get; private set; }

        // Movement Toggle
        public bool CanMove { get; private set; } = true;

        private Dictionary<string, bool> values = new Dictionary<string, bool>();

        private void OnLook(InputValue value)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 targetPosition = hit.point; // Store hit position
                targetPosition.y = transform.position.y; // Keep it level with player

                LookTarget = targetPosition; // Assign the modified value

                // Debug.Log(
                //     $"🎯 Pointing at: {hit.collider.gameObject.name}, Target Position: {LookTarget}"
                // );
            }
        }

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

        private void OnRightClick(InputValue value)
        {
            if (playerInventory == null)
            {
                Debug.LogError("[PlayerInputHandler] PlayerInventory is not assigned!");
                return;
            }

            if (playerInventory.EquippedWeapon == null)
            {
                Debug.Log("[PlayerInputHandler] Equip a weapon first!");
                return;
            }

            bool isPressed = value.isPressed;

            if (playerInventory.EquippedWeapon is MeleeWeapon)
            {
                IsBlock = isPressed;
                Debug.Log($"[PlayerInputHandler] Blocking: {IsBlock}");
                playerCombat.RightClickHandler("Block", isPressed);
            }
            else if (playerInventory.EquippedWeapon is RangedWeapon)
            {
                IsAim = isPressed;
                Debug.Log($"[PlayerInputHandler] Aiming: {IsAim}");
                playerCombat.RightClickHandler("Aim", isPressed);
            }
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
            IsRangedDraw = value;
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

        // private void OnDrawGizmos()
        // {
        //     if (Mouse.current != null)
        //     {
        //         // Get mouse position in screen space
        //         Vector2 mousePosition = Mouse.current.position.ReadValue();

        //         // Convert to world position using a ray
        //         Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        //         if (Physics.Raycast(ray, out RaycastHit hit))
        //         {
        //             // Player's position
        //             Vector3 playerPosition = transform.position;

        //             // Target point where the mouse is pointing
        //             Vector3 lookTarget = hit.point;
        //             lookTarget.y = playerPosition.y; // Keep it level with the player

        //             // Set Gizmo color
        //             Gizmos.color = Color.red;

        //             // Draw a directional line in Scene View
        //             Gizmos.DrawLine(playerPosition, lookTarget);
        //             Gizmos.DrawSphere(lookTarget, 0.1f);

        //             // 🔥 Draw a temporary line in **Game View** (disappears after 0.5s)
        //             Debug.DrawLine(playerPosition, lookTarget, Color.red, 0.5f);
        //         }
        //     }
        // }
    }
}
