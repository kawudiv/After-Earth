using Player.Base;
using Player.States.Combat.Melee;
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
        }

        public bool IsEquip()
        {
            return playerInventory.EquippedMeleeWeapon != null;
        }

        public bool IsAttacking()
        {
            return player.StateMachine.CurrentState is AttackMeleeState;
        }

        public void StartAttack()
        {
            if (!IsAttacking())
            {
                player.StateMachine.ChangeState(new AttackMeleeState(player, player.StateMachine));
            }
        }

        public void PerformLightAttack()
        {
            if (!IsEquip())
                return;
            player.PlayerAnimation.SetTrigger("MeleeAttack");
            // Call BaseWeapon's Attack() method
            playerInventory.EquippedMeleeWeapon?.Attack();
        }
    }
}
