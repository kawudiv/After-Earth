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

        private void Awake()
        {
            player = GetComponent<PlayerBase>();
            inputHandler = GetComponent<PlayerInputHandler>();
            playerInventory = GetComponent<PlayerInventory>();
            Debug.Log("[PlayerCombat] Initialized.");
        }

        // Checks if the player has a weapon equipped
        public bool IsEquip()
        {
            bool isEquipped = playerInventory?.EquippedWeapon != null;
            Debug.Log($"[PlayerCombat] IsEquip: {isEquipped}");
            return isEquipped;
        }

        // Checks if the player is currently in an attack state
        public bool IsAttacking()
        {
            bool isAttacking = player.StateMachine.CurrentState is AttackWeaponState;
            Debug.Log($"[PlayerCombat] IsAttacking: {isAttacking}");
            return isAttacking;
        }

        // Starts an attack if conditions are met
        public void StartAttack()
        {
            Debug.Log("[PlayerCombat] Attempting to start attack...");

            if (!IsEquip())
            {
                Debug.LogWarning("[PlayerCombat] Cannot attack: No weapon equipped.");
                return;
            }

            // if (!player.IsWeaponDrawn)
            // {
            //     Debug.LogWarning("[PlayerCombat] Cannot attack: Weapon is not drawn.");
            //     return;
            // }

            if (IsAttacking())
            {
                Debug.LogWarning("[PlayerCombat] Already attacking.");
                return;
            }

            Debug.Log("[PlayerCombat] Switching to AttackWeaponState.");
            player.StateMachine.ChangeState(new AttackWeaponState(player, player.StateMachine));
        }

        // Performs an attack animation and triggers weapon logic
        public void PerformLightAttack()
        {
            Debug.Log("[PlayerCombat] Performing light attack...");

            if (!IsEquip())
            {
                Debug.LogWarning("[PlayerCombat] Cannot attack: No weapon equipped.");
                return;
            }

            // if (!player.IsWeaponDrawn)
            // {
            //     Debug.LogWarning("[PlayerCombat] Cannot attack: Weapon is not drawn.");
            //     return;
            // }

            if (playerInventory.EquippedWeapon is MeleeWeapon meleeWeapon)
            {
                Debug.Log("[PlayerCombat] Performing melee attack.");
                player.PlayerAnimation.SetTrigger("MeleeAttack");
                meleeWeapon.Attack();
            }
            else if (playerInventory.EquippedWeapon is RangedWeapon rangedWeapon)
            {
                Debug.Log("[PlayerCombat] Performing ranged attack.");
                player.PlayerAnimation.SetTrigger("Shoot");
                rangedWeapon.Attack();
            }
            else
            {
                Debug.LogWarning("[PlayerCombat] Equipped weapon type is unknown.");
            }
        }
    }
}
