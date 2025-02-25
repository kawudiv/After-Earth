using UnityEngine;
using Weapons.Types;

namespace Weapons.Components.Melee
{
    public class SwordWeapon : MeleeWeapon
    {
        private void Awake()
        {
            // Set default values for the sword.
            weaponName = "Sword";
            damage = 10f; // Base damage of the sword.
            attackSpeed = 1.2f; // Time between attacks.
            attackRange = 1.5f; // Effective range of the sword.

            // Optionally, assign or validate the attackPoint.
            if (attackPoint == null)
            {
                // If not set in the Inspector, default to this object's transform.
                attackPoint = transform;
            }

            // Optionally set the hitbox radius.
            hitboxRadius = 1.5f;
        }

        // Optionally override the Attack() method to add sword-specific behavior.
        public override void Attack()
        {
            // Call the base method to perform standard melee attack logic.
            base.Attack();

            // Additional sword-specific effects (e.g., play sound, spawn particle effects, etc.)
            // For example:
            Debug.Log($"{weaponName} slashes with a special effect!");
        }
    }
}
