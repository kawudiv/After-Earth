using UnityEngine;
using Weapons.Types; // Make sure this matches your namespace structure

namespace Weapons.Components.Ranged
{
    public class PistolWeapon : RangedWeapon
    {
        private void Awake()
        {
            // Set default values for the pistol.
            weaponName = "Pistol";
            damage = 20f; // Base damage provided by the pistol.
            attackSpeed = 0.5f; // Time between shots (faster firing rate for a pistol).
            attackRange = 50f; // Effective range of the pistol.

            bulletSpeed = 30f; // Speed at which the bullet travels.
            bulletLifeTime = 3f; // Lifetime of the bullet before it's destroyed.

            // Ensure that the attackPoint is set in the Inspector;
            // otherwise, default to this transform.
            if (attackPoint == null)
            {
                attackPoint = transform;
            }
        }

        public override void Attack()
        {
            // Optionally, add any pistol-specific effects here (e.g., muzzle flash, sound effects, recoil).
            base.Attack(); // Call the base RangedWeapon.Attack() to spawn the bullet.
            Debug.Log("Pistol fired!");
        }
    }
}
