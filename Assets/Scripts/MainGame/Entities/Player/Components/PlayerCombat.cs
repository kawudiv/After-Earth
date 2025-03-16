using Constraints.TargetTracking;
using Player.Base;
using Player.States.Combat.Common;
using UnityEngine;
using Weapons.Types;

namespace Player.Components
{
    public class PlayerCombat : MonoBehaviour
    {
        private PlayerBase player;
        private PlayerInputHandler inputHandler;
        private PlayerInventory playerInventory;
        public PlayerInventory PlayerInventory => playerInventory;

        private TargetTrackingController targetTrackingController;

        private void Awake()
        {
            player = GetComponent<PlayerBase>();
            inputHandler = GetComponent<PlayerInputHandler>();
            playerInventory = GetComponent<PlayerInventory>();
            targetTrackingController = FindAnyObjectByType<TargetTrackingController>();
            Debug.Log("[PlayerCombat] Initialized.");
        }

        // Checks if the player has a weapon equipped
        public bool IsEquip()
        {
            bool isEquipped = playerInventory?.EquippedWeapon != null;
            Debug.Log($"[PlayerCombat] IsEquip: {isEquipped}");
            return isEquipped;
        }

        public void RightClickHandler(string actionType, bool isPressed)
        {
            Debug.Log(
                $"[PlayerCombat] RightClickHandler called - Action: {actionType}, IsPressed: {isPressed}"
            );

            if (!IsEquip())
            {
                Debug.LogError($"[PlayerCombat] Cannot perform {actionType}: No weapon equipped.");
                return;
            }

            switch (actionType)
            {
                case "Block":
                    inputHandler.IsBlock = isPressed;
                    player.PlayerAnimation.SetBool("IsBlocking", isPressed);
                    Debug.Log($"[PlayerCombat] Block action set to {inputHandler.IsBlock}");
                    break;

                case "Aim":
                    inputHandler.IsAim = isPressed;
                    player.PlayerAnimation.SetBool("IsAiming", isPressed);
                    Debug.Log($"[PlayerCombat] Aim action set to {inputHandler.IsAim}");

                    if (targetTrackingController != null)
                    {
                        Debug.Log(
                            $"[PlayerCombat] TargetTrackingController found. Updating tracking..."
                        );

                        if (isPressed) // Aiming
                        {
                            RangedWeapon rangedWeapon =
                                playerInventory.EquippedWeapon as RangedWeapon;

                            if (rangedWeapon != null)
                            {
                                Debug.Log(
                                    $"[PlayerCombat] Ranged weapon detected: {rangedWeapon.WeaponName}"
                                );
                                Debug.Log(
                                    $"[PlayerCombat] Applying Tracking Weights - Gun: {rangedWeapon.gunTrackingWeight}, Body: {rangedWeapon.bodyTrackingWeight}"
                                );

                                // ✅ Apply weapon-specific tracking settings
                                targetTrackingController.InitializeWeaponIK(rangedWeapon);
                                targetTrackingController.SetAiming(true);
                            }
                            else
                            {
                                Debug.LogWarning("[PlayerCombat] No RangedWeapon equipped!");
                            }
                        }
                        else // Stop Aiming
                        {
                            Debug.Log("[PlayerCombat] Resetting Tracking to default.");
                            targetTrackingController.SetAiming(false);
                        }
                    }
                    else
                    {
                        Debug.LogError(
                            "[PlayerCombat] TargetTrackingController is NULL! Cannot update tracking."
                        );
                    }
                    break;

                default:
                    Debug.LogWarning($"[PlayerCombat] Unknown action: {actionType}");
                    break;
            }
        }

        // Checks if the player is currently in an attack state
        public bool IsAttacking()
        {
            bool isAttacking = player.StateMachine.CurrentState is AttackWeaponState;
            Debug.Log($"[PlayerCombat] IsAttacking: {isAttacking}");
            return isAttacking;
        }

        public void StartAttack()
        {
            Debug.Log("[PlayerCombat] Attempting to start attack...");

            if (!IsEquip())
            {
                Debug.LogWarning("[PlayerCombat] Cannot attack: No weapon equipped.");
                return;
            }

            if (IsAttacking())
            {
                Debug.LogWarning("[PlayerCombat] Already attacking.");
                return;
            }

            Debug.Log("[PlayerCombat] Switching to AttackWeaponState.");
            player.StateMachine.ChangeState(new AttackWeaponState(player, player.StateMachine));
        }

        public void PerformAttack()
        {
            if (!IsEquip())
                return;

            if (playerInventory.EquippedWeapon is MeleeWeapon)
            {
                PerformMeleeAttack();
            }
            else if (playerInventory.EquippedWeapon is RangedWeapon)
            {
                PerformRangedAttack();
            }
        }

        private void PerformMeleeAttack()
        {
            Debug.Log("[PlayerCombat] Performing melee attack.");
            player.PlayerAnimation.SetTrigger("MeleeAttack");
            (playerInventory.EquippedWeapon as MeleeWeapon)?.Attack();
        }

        private void PerformRangedAttack()
        {
            Debug.Log("[PlayerCombat] Performing ranged attack.");
            player.PlayerAnimation.SetTrigger("RangedAttack");
            (playerInventory.EquippedWeapon as RangedWeapon)?.Attack();
        }

        // ✅ Called from Animation Event (Start of attack)
        public void EnableWeaponCollider()
        {
            if (playerInventory.EquippedWeapon is MeleeWeapon meleeWeapon)
            {
                meleeWeapon.EnableWeaponCollider();
            }
            else
            {
                Debug.LogWarning("[PlayerCombat] No melee weapon equipped!");
            }
        }

        // ✅ Called from Animation Event (End of attack)
        public void DisableWeaponCollider()
        {
            if (playerInventory.EquippedWeapon is MeleeWeapon meleeWeapon)
            {
                meleeWeapon.DisableWeaponCollider();
            }
            else
            {
                Debug.LogWarning("[PlayerCombat] No melee weapon equipped!");
            }
        }
    }
}
