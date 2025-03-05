using Player.Base;
using Player.Components;
using UnityEngine;

namespace Player.States.Combat.Common
{
    public class AttackWeaponState : BaseWeaponState
    {
        private PlayerCombat playerCombat;
        private float timePassed;
        private float clipLength;
        private float clipSpeed;

        public AttackWeaponState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine)
        {
            playerCombat = player.PlayerCombat;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("[AttackWeaponState] Entered Weapon Attack State");

            timePassed = 0f; // Reset time tracking
            playerCombat.PerformLightAttack(); // Trigger attack
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            timePassed += Time.deltaTime;

            int currentLayer = 2; // ✅ Using direct reference instead of a constant
            AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(currentLayer);

            if (!stateInfo.IsTag("Attack")) // ✅ Uses Animator Tags instead of hardcoded state names
            {
                Debug.LogWarning(
                    $"[AttackWeaponState] Unexpected state detected (Hash: {stateInfo.fullPathHash}). Exiting..."
                );
                stateMachine.ChangeState(player.IdleState);
                return;
            }

            if (stateInfo.length > 0)
            {
                clipLength = stateInfo.length;
                clipSpeed = stateInfo.speed;
            }
            else
            {
                Debug.LogWarning("[AttackWeaponState] No valid attack animation detected.");
                stateMachine.ChangeState(player.IdleState);
                return;
            }

            if (timePassed >= clipLength / clipSpeed)
            {
                if (playerCombat.IsAttacking()) // If still attacking, continue the combo
                {
                    stateMachine.ChangeState(player.AttackWeaponState);
                }
                else
                {
                    stateMachine.ChangeState(player.IdleState);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("[AttackWeaponState] Exiting Weapon Attack State");
        }

        protected override void HandleWeaponAction()
        {
            // Additional weapon-specific logic if needed
        }
    }
}
