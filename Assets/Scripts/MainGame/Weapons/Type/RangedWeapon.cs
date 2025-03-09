using Core;
using UnityEngine;
using Weapons.Base;

namespace Weapons.Types
{
    public class RangedWeapon : WeaponBase
    {
        public Transform attackPoint;

        [Header("Bullet Settings")]
        [SerializeField]
        protected float bulletSpeed;

        [SerializeField]
        protected float bulletLifeTime;

        [SerializeField]
        protected float bulletForce;

        [Header("Weapon Layers")]
        [SerializeField]
        protected LayerMask enemyLayers;

        [SerializeField]
        protected LayerMask environmentLayers;

        [Header("Weapon Effects")]
        [SerializeField]
        protected GameObject impactEffectPrefab;

        public int rangeID;

        public override void Attack()
        {
            base.Awake();
            weaponTypeID = rangeID;

            if (attackPoint == null)
            {
                Debug.LogWarning($"{weaponName} has no attackPoint assigned!");
                return;
            }

            RaycastHit hit;
            if (
                Physics.Raycast(
                    attackPoint.position,
                    attackPoint.forward,
                    out hit,
                    attackRange,
                    enemyLayers | environmentLayers
                )
            )
            {
                Debug.Log($"{weaponName} hit {hit.collider.name}!");

                // Apply damage if it's an enemy
                if (hit.collider.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damage);
                }

                // Apply impact force
                BulletImpact(hit, bulletForce);

                // Spawn impact effect if it hits an environment layer
                if (
                    impactEffectPrefab != null
                    && (environmentLayers & (1 << hit.collider.gameObject.layer)) != 0
                )
                {
                    Instantiate(impactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            else
            {
                Debug.Log($"{weaponName} missed!");
            }
        }

        // ✅ Handles bullet impact force on rigidbodies
        private void BulletImpact(RaycastHit hit, float bulletForce)
        {
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * bulletForce, ForceMode.Impulse);
                Debug.Log($"Applied {bulletForce} force to {hit.collider.name}");
            }
        }

        // Debugging: Draw ray in Scene View
        private void OnDrawGizmos()
        {
            if (attackPoint != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(attackPoint.position, attackPoint.forward * attackRange);
            }
        }
    }
}
