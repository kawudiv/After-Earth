using Player.Base;
using Player.Components;
using UnityEngine;

namespace Player.States.Combat.Melee
{
    public class AttackMeleeState : BaseWeaponState
    {
        private PlayerCombat playerCombat;

        public AttackMeleeState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine)
        {
            playerCombat = player.GetComponent<PlayerCombat>();
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("[AttackMeleeState] Entered Melee Attack State");

            // Call attack execution from PlayerCombat
            playerCombat.PerformLightAttack();
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("[AttackMeleeState] Exiting Melee Attack State");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Transition back to idle or movement state after attack finishes
            if (!playerCombat.IsAttacking())
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
