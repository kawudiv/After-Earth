using UnityEngine;
using Weapons.Base;

namespace Player.Base
{
    public abstract class BaseMeleeState : State
    {
        // Reference to the equipped melee weapon.
        protected WeaponBase equippedWeapon;

        // Common initialization for melee states.
        protected BaseMeleeState(PlayerBase player, StateMachine stateMachine)
            : base(player, stateMachine)
        {
            // Assuming your PlayerBase has a property for its melee weapon.
            equippedWeapon = player.EquippedMeleeWeapon;
        }

        // You can add common utility methods here that every melee state might need.
        // For example: handling combo counters, playing shared sounds, etc.
    }
}
