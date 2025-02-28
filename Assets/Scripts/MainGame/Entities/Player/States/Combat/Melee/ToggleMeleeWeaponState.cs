using Player.Base;
using UnityEngine;

namespace Player.States.Combat.Melee
{
    public class ToggleMeleeWeaponState : BaseMeleeState
    {
        // Duration of the draw/sheath animation.
        private float toggleTime;
        private const float defaultToggleDuration = 0.5f; // Adjust based on your animation length

        public ToggleMeleeWeaponState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("[ToggleMeleeWeaponState] Entered ToggleMeleeWeaponState");

            if (player.PlayerInputHandler.IsMeleeDraw)
            {
                player.PlayerInputHandler.ClearMeleeDraw(); // Prevents retriggering

                // ✅ Fix: Check if already drawn before triggering animations
                if (player.IsWeaponDrawn)
                {
                    player.PlayerAnimation.SetTrigger("Sheath");
                    Debug.Log("[ToggleMeleeWeaponState] Sheathing weapon...");
                }
                else
                {
                    player.PlayerAnimation.SetTrigger("DrawMelee");
                    Debug.Log("[ToggleMeleeWeaponState] Drawing weapon...");
                }

                // Set timer based on animation duration
                toggleTime = defaultToggleDuration;
            }
            else
            {
                Debug.Log("[ToggleMeleeWeaponState] No melee draw input detected, exiting state.");
                stateMachine.ChangeState(player.IdleState);
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            toggleTime -= Time.deltaTime;
            if (toggleTime <= 0f)
            {
                // ✅ Fix: Update state after animation finishes
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
    }
}
