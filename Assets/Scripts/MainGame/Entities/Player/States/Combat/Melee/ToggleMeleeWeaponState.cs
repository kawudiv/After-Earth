using UnityEngine;
using Player.Base;

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

            // Check the input flag from the PlayerInputHandler.
            // (This flag is set when the player presses key "1" for a melee draw toggle.)
            if (player.PlayerInputHandler.IsMeleeDraw)
            {
                // Once we register the input, immediately clear it to prevent re-triggering.
                player.PlayerInputHandler.ClearMeleeDraw();  // <-- We'll add this helper method to PlayerInputHandler.

                // Depending on whether the weapon is already drawn, trigger the appropriate animation.
                if (player.IsWeaponDrawn)
                {
                    player.PlayerAnimation.SetTrigger("SheathMelee");
                    Debug.Log("[ToggleMeleeWeaponState] SheathMelee triggered.");
                }
                else
                {
                    player.PlayerAnimation.SetTrigger("DrawMelee");
                    Debug.Log("[ToggleMeleeWeaponState] DrawMelee triggered.");
                    Debug.Log(player.IsWeaponDrawn);
                }
                // Set the timer for the duration of the animation.
                toggleTime = defaultToggleDuration;
            }
            else
            {
                // If no melee draw input is detected, return to Idle.
                Debug.Log("[ToggleMeleeWeaponState] No melee draw input detected, exiting state.");
                stateMachine.ChangeState(player.IdleState);
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Decrease the timer.
            toggleTime -= Time.deltaTime;
            if (toggleTime <= 0f)
            {
                // Toggle the drawn state.
                player.IsWeaponDrawn = !player.IsWeaponDrawn;
                Debug.Log($"[ToggleMeleeWeaponState] Toggle complete. IsWeaponDrawn: {player.IsWeaponDrawn}");
                // Transition back to Idle (which should now display the correct (armed or unarmed) idle pose).
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
