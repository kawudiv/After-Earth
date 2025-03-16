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
                if (timePassed >= 0.3f) // Adjust based on fire rate
                {
                    Debug.Log("[AttackWeaponState] 🎯 Ranged attack finished. Returning to Idle.");
                    stateMachine.ChangeState(player.IdleState);
                }
                return;
            }

            int currentLayer = 2;
            float layerWeight = player.animator.GetLayerWeight(currentLayer);

            if (layerWeight == 0)
            {
                Debug.Log(
                    "[AttackWeaponState] ❌ Layer 2 is inactive! Cannot detect attack animations."
                );
                return;
            }

            AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(currentLayer);
            bool isNewState = stateInfo.shortNameHash == 0;
            if (isNewState)
            {
                Debug.Log(
                    "[AttackWeaponState] ❌ 'New State' detected. Waiting for valid animation..."
                );
                return;
            }

            int meleeHash = Animator.StringToHash("Melee");
            bool isInMeleeBlendTree = stateInfo.shortNameHash == meleeHash;

            if (isInMeleeBlendTree || stateInfo.IsTag("Attack"))
            {
                Debug.Log(
                    "[AttackWeaponState] ✅ Detected 'Melee' Blend Tree or 'Attack' animation."
                );
            }
            else
            {
                Debug.Log("[AttackWeaponState] ❌ No valid attack animation detected.");
            }

            if (!stateInfo.IsTag("Attack"))
            {
                Debug.Log("[AttackWeaponState] ⚠️ Attack animation ended. Switching to Idle.");
                stateMachine.ChangeState(player.IdleState);
                return;
            }

            if (stateInfo.length > 0)
            {
                clipLength = stateInfo.length;
                clipSpeed = stateInfo.speed;
                Debug.Log(
                    $"[AttackWeaponState] 🎥 Clip Length: {clipLength:F2}s, Speed: {clipSpeed:F2}"
                );
            }

            if (timePassed >= clipLength / clipSpeed)
            {
                Debug.Log("[AttackWeaponState] ⏹ Attack animation finished. Returning to Idle.");
                stateMachine.ChangeState(player.IdleState);
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
