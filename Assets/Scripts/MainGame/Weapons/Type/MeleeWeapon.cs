using Core;
using UnityEngine;
using Weapons.Base;

namespace Weapons.Types
{
    public class MeleeWeapon : WeaponBase
    {
        // The point from which the attack originates (for example, the tip of a sword).
        public Transform attackPoint;

        // The radius of the hitbox.
        public float hitboxRadius = 1.5f;

        // The layer mask that specifies which layers are considered valid targets.
        public LayerMask targetLayers;
        public int meleeID;

        public override void Attack()
        {
            // Assign melee ID to weaponTypeID (so animation system can use it)
            weaponTypeID = meleeID;

            // Use OverlapSphere to detect all colliders in the attack area.
            Collider[] hitColliders = Physics.OverlapSphere(
                attackPoint.position,
                hitboxRadius,
                targetLayers
            );

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out IDamageable damageable))
                {
                    float finalDamage = damage;
                    damageable.TakeDamage(finalDamage);
                    Debug.Log(
                        $"{weaponName} attacked {hitCollider.name} for {finalDamage} damage."
                    );
                }
            }
        }

        // Draw a visual representation of the attack hitbox in the Unity Editor.
        private void OnDrawGizmos()
        {
            if (attackPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(attackPoint.position, hitboxRadius);
            }
        }
    }
}
