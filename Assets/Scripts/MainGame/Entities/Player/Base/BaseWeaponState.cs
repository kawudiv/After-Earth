using UnityEngine;
using Weapons.Base;

namespace Player.Base
{
    public abstract class BaseWeaponState : State
    {
        // Reference to the equipped weapon (could be melee or ranged).
        protected WeaponBase equippedWeapon;

        // Common initialization for weapon states.
        protected BaseWeaponState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine)
        {
            // Initialize the equipped weapon (melee or ranged).
            equippedWeapon = player.EquippedWeapon;
        }

        // Abstract method to be implemented by the derived classes.
        // Allows each state to define its specific behavior for handling weapon actions.
        protected abstract void HandleWeaponAction();

        // Common utility method for handling combo counters, sounds, etc.
        protected void PlayWeaponSound()
        {
            // Check if the weapon has a sound associated with it.
            //equippedWeapon?.PlaySound();
        }

        // You can add additional methods for handling ranged or melee-specific actions here.
    }
}
