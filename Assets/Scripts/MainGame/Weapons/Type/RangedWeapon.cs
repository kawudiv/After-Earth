using Core;
using UnityEngine;
using Weapons.Base;

namespace Weapons.Types
{
    public class RangedWeapon : WeaponBase
    {
        // The point from which the bullet originates.
        public Transform attackPoint;

        // The bullet prefab to be instantiated when firing.
        public GameObject bulletPrefab;

        // The speed at which the bullet travels.
        public float bulletSpeed = 20f;

        // The lifetime of the bullet before it is destroyed.
        public float bulletLifeTime = 5f;

        // Unique ranged weapon ID (for animation blending if needed).
        public int rangeID;

        public override void Attack()
        {
            // Assign ranged ID to weaponTypeID (for animation system use).
            weaponTypeID = rangeID;

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

            // Apply velocity if the bullet has a Rigidbody component.
            if (bulletInstance.TryGetComponent(out Rigidbody rb))
            {
                rb.linearVelocity = attackPoint.forward * bulletSpeed;
            }
            else
            {
                Debug.LogWarning("Bullet prefab does not have a Rigidbody component.");
            }

            // Destroy the bullet after a certain time to prevent memory leaks.
            Destroy(bulletInstance, bulletLifeTime);

            Debug.Log($"{weaponName} fired a bullet!");
        }

        // Draw a visual representation of the attack point in the Unity Editor.
        private void OnDrawGizmos()
        {
            if (attackPoint != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(attackPoint.position, 0.1f);
            }
        }
    }
}
