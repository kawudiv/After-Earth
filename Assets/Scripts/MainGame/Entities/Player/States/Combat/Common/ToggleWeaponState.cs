using Player.Base;
using UnityEngine;

namespace Player.States.Combat.Common
{
    public class ToggleWeaponState : BaseWeaponState
    {
        private float toggleTime;
        private const float defaultToggleDuration = 0.5f;

        public ToggleWeaponState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("[ToggleMeleeWeaponState] Entered ToggleMeleeWeaponState");

            // ✅ Check if the player has a melee weapon equipped
            if (player.PlayerInventory.EquippedMeleeWeapon == null)
            {
                Debug.LogWarning("[ToggleMeleeWeaponState] No melee weapon equipped! Cannot draw.");
                stateMachine.ChangeState(player.IdleState); // Return to Idle if no weapon is equipped
                return;
            }

            bool isMeleeDraw = player.PlayerInputHandler.IsMeleeDraw;
            bool isRangedDraw = player.PlayerInputHandler.IsRangedDraw;

            if (isMeleeDraw || isRangedDraw)
            {
                player.PlayerInputHandler.ClearDraw();

                // Determine animation trigger
                string animationTrigger = GetDrawSheathTrigger(isMeleeDraw);
                player.PlayerAnimation.SetTrigger(animationTrigger);
                Debug.Log($"[ToggleMeleeWeaponState] Triggered: {animationTrigger}");

                toggleTime = defaultToggleDuration;
            }
            else
            {
                Debug.Log("[ToggleMeleeWeaponState] No valid draw input detected, exiting state.");
                stateMachine.ChangeState(player.IdleState);
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            toggleTime -= Time.deltaTime;
            if (toggleTime <= 0f)
            {
                player.IsWeaponDrawn = !player.IsWeaponDrawn;
                Debug.Log(
                    $"[ToggleMeleeWeaponState] Toggle complete. IsWeaponDrawn: {player.IsWeaponDrawn}"
                );
                stateMachine.ChangeState(player.IdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("[ToggleMeleeWeaponState] Exiting ToggleMeleeWeaponState");
        }

        protected override void HandleWeaponAction() { }

        private string GetDrawSheathTrigger(bool isMelee)
        {
            if (player.IsWeaponDrawn)
                return "Sheath";

            return isMelee ? "DrawMelee" : "DrawRanged";
        }
    }
}
