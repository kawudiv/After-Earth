using Core;
using UnityEngine;
using Weapons.Base;

namespace Weapons.Types
{
    public class RangedWeapon : WeaponBase
    {
        public Transform attackPoint;

        // The prefab for the bullet. Set this in the Inspector.
        public GameObject bulletPrefab;

        // The speed at which the bullet travels.
        public float bulletSpeed = 20f;

        // Optional: how long before the bullet is destroyed.
        public float bulletLifeTime = 5f;

        public override void Attack()
        {
            if (bulletPrefab == null || attackPoint == null)
            {
                Debug.LogWarning(
                    $"{weaponName} cannot attack because bulletPrefab or attackPoint is not assigned."
                );
                return;
            }

            // Instantiate a bullet at the attack point's position and rotation.
            GameObject bulletInstance = Instantiate(
                bulletPrefab,
                attackPoint.position,
                attackPoint.rotation
            );

            // Attempt to get a Rigidbody component to apply physics.
            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Set the bullet's velocity so that it moves forward.
                rb.linearVelocity = attackPoint.forward * bulletSpeed;
            }
            else
            {
                Debug.LogWarning("Bullet prefab does not have a Rigidbody component.");
            }

            // Optionally, destroy the bullet after a certain time.
            Destroy(bulletInstance, bulletLifeTime);
        }
    }
}
