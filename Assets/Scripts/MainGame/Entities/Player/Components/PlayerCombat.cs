using Player.Base;
using Player.States.Combat.Melee;
using UnityEngine;

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
        }

        /// <summary>
        /// Checks if the player has a weapon equipped.
        /// </summary>
        public bool IsEquip()
        {
            if (playerInventory.EquippedMeleeWeapon == null)
            {
                Debug.Log("[PlayerCombat] Cannot attack: No melee weapon equipped.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if the player is currently attacking.
        /// </summary>
        public bool IsAttacking()
        {
            return player.StateMachine.CurrentState is AttackMeleeState;
        }

        /// <summary>
        /// Starts an attack by changing the state to AttackMeleeState.
        /// </summary>
        public void StartAttack()
        {
            if (!IsAttacking())
            {
                player.StateMachine.ChangeState(new AttackMeleeState(player, player.StateMachine));
            }
        }

        private void AttackDamage()
        {
            Debug.Log("[PlayerCombat] Calculating attack damage.");
        }

        public void PerformLightAttack()
        {
            if (!IsEquip())
            {
                Debug.Log("[PlayerCombat] Cannot perform light attack: No weapon equipped.");
                return;
            }

            Debug.Log("[PlayerCombat] Performing Light Attack!");
            AttackDamage();
        }
        public void PerformHeavyAttack()
        {
            if (!IsEquip())
            {
                Debug.Log("[PlayerCombat] Cannot perform heavy attack: No weapon equipped.");
                return;
            }

            Debug.Log("[PlayerCombat] Performing Heavy Attack!");
            AttackDamage();
        }

        /// <summary>
        /// Handles blocking (only for melee weapons).
        /// </summary>
        public void Block()
        {
            if (!IsEquip())
            {
                Debug.Log("[PlayerCombat] Cannot block: No weapon equipped.");
                return;
            }

            if (playerInventory.EquippedMeleeWeapon != null)
            {
                Debug.Log("[PlayerCombat] Blocking with melee weapon.");
            }
            else
            {
                Debug.Log("[PlayerCombat] Cannot block: Ranged weapon equipped.");
            }
        }
    }
}
