using Player.Base;
using Player.Components;
using UnityEngine;
using Weapons.Types;

namespace Player.States.Combat.Common
{
    public class AttackWeaponState : BaseWeaponState
    {
        private PlayerCombat playerCombat;
        private PlayerInventory playerInventory;
        private float timePassed;
        private float clipLength;
        private float clipSpeed;

        public AttackWeaponState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine)
        {
            playerCombat = player.PlayerCombat;
            playerInventory = player.PlayerInventory;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("[AttackWeaponState] ✅ Entered Weapon Attack State");

            timePassed = 0f; // Reset time tracking
            playerCombat.PerformAttack(); // Trigger attack

            Debug.Log($"[AttackWeaponState] 🔄 Starting Attack - Time Passed: {timePassed}");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            timePassed += Time.deltaTime;

            Debug.Log($"[AttackWeaponState] ⏳ Time Passed: {timePassed:F2}s");

            if (playerInventory.EquippedWeapon is RangedWeapon)
            {
                HandleRangedAttack();
            }
            else if (playerInventory.EquippedWeapon is MeleeWeapon)
            {
                HandleMeleeAttack();
            }
        }

        private void HandleRangedAttack()
        {
            if (timePassed >= 0.3f) // Adjust based on fire rate
            {
                Debug.Log("[AttackWeaponState] 🎯 Ranged attack finished. Returning to Idle.");
                stateMachine.ChangeState(player.IdleState);
            }
        }

        private void HandleMeleeAttack()
        {
           
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("[AttackWeaponState] ❌ Exiting Weapon Attack State");
        }

        protected override void HandleWeaponAction()
        {
            Debug.Log("[AttackWeaponState] 🛠 Handling Weapon Action (if applicable)");
        }
    }
}
