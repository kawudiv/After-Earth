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
        private bool isAttacking;

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
            isAttacking = true;
            timePassed = 0f; // Reset time tracking
            playerCombat.PerformAttack(); // Trigger attack

            Debug.Log($"[AttackWeaponState] 🔄 Starting Attack - Time Passed: {timePassed}");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            timePassed += Time.deltaTime;

            //Debug.Log($"[AttackWeaponState] ⏳ Time Passed: {timePassed:F2}s");

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
                isAttacking = false;
                Debug.Log("[AttackWeaponState] 🎯 Ranged attack finished. Returning to Idle.");
                stateMachine.ChangeState(player.IdleState);
            }
        }

        private void HandleMeleeAttack()
        {
            if (playerInventory.EquippedWeapon is MeleeWeapon meleeWeapon)
            {
                float attackDuration = 1f / meleeWeapon.attackSpeed; // Attack duration based on attack speed

                if (timePassed >= attackDuration)
                {
                    isAttacking = false;
                    Debug.Log(
                        $"[AttackWeaponState] ⚔️ Melee attack finished after {attackDuration:F2}s. Returning to Idle."
                    );
                    stateMachine.ChangeState(player.IdleState);
                }
            }
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
