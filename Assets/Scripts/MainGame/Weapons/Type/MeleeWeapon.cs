using System.Collections.Generic;
using Core;
using EnemyAi.Test;
using UnityEngine;
using Weapons.Base;

namespace Weapons.Types
{
    public class MeleeWeapon : WeaponBase
    {
        public Transform attackPoint;

        [Header("Hitbox Settings")]
        [SerializeField]
        protected float hitboxRadius = 1.5f;

        [SerializeField]
        protected LayerMask enemyLayers;

        [Header("Physics Settings")]
        [SerializeField]
        protected float impactForce = 10f; // Force applied to ragdoll

        [Header("Weapon Effects")]
        [SerializeField]
        protected GameObject impactEffectPrefab; // Slash effect

        [SerializeField]
        protected GameObject hitEffectPrefab; // Enemy hit effect

        public int meleeID;
        private Collider hitbox;

        protected override void Awake()
        {
            base.Awake();
            weaponTypeID = meleeID;
            ConfigureSwordCollider();
        }

        public override void Attack()
        {
            Debug.Log($"{weaponName} is attacking!");

            // ✅ Spawn weapon trail effect
            if (impactEffectPrefab != null)
            {
                Instantiate(impactEffectPrefab, attackPoint.position, attackPoint.rotation);
            }

            // ✅ Perform melee hit detection
            Collider[] hitEnemies = Physics.OverlapSphere(
                attackPoint.position,
                hitboxRadius,
                enemyLayers
            );

            if (hitEnemies.Length == 0)
            {
                Debug.Log("❌ No enemies hit!");
                return;
            }

            // ✅ Track already damaged enemies
            HashSet<IDamageable> damagedEnemies = new HashSet<IDamageable>();

            foreach (Collider enemy in hitEnemies)
            {
                Debug.Log($"🎯 Hit detected on: {enemy.name}");

                // ✅ Get the root object
                Transform root = enemy.transform.root;

                if (root.TryGetComponent<IDamageable>(out var damageable))
                {
                    if (!damagedEnemies.Contains(damageable)) // ✅ Avoid double damage
                    {
                        Debug.Log($"⚔️ {weaponName} dealing {damage} damage to {root.name}");
                        damageable.TakeDamage(damage);
                        damagedEnemies.Add(damageable); // ✅ Mark as damaged
                    }
                }
                else
                {
                    Debug.Log($"❌ {root.name} does NOT implement IDamageable!");
                }

                // ✅ Call SwordImpact() to handle physics separately
                SwordImpact(enemy);
            }
        }

        private void SwordImpact(Collider enemy)
        {
            // ✅ First, check if the enemy has a health system
            EnemyHealth enemyHealth = enemy.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null && enemyHealth.isDead) // ✅ Only ragdoll if the enemy is dead
            {
                TestRagdoll ragdoll = enemy.GetComponentInParent<TestRagdoll>();
                if (ragdoll != null)
                {
                    Vector3 hitPoint = enemy.ClosestPoint(attackPoint.position);
                    Vector3 forceDirection =
                        (enemy.transform.position - attackPoint.position).normalized * impactForce;

                    ragdoll.TriggerRagdoll(); // ✅ Activate ragdoll first
                    ragdoll.ApplyForceToRagdoll(hitPoint, forceDirection); // ✅ Apply force after ragdoll

                    //Debug.Log($"💀 {enemy.name} is dead! Ragdoll activated & force applied.");
                }
            }
            else if (enemy.TryGetComponent(out Rigidbody rb))
            {
                // ✅ Apply force if there's no ragdoll (for non-ragdoll enemies)
                Vector3 forceDirection =
                    (enemy.transform.position - attackPoint.position).normalized * impactForce;
                rb.AddForce(forceDirection, ForceMode.Impulse);
                Debug.Log($"💥 Knockback applied to {enemy.name}!");
            }

            // ✅ Spawn hit effect
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, enemy.transform.position, Quaternion.identity);
            }
        }

        private void ConfigureSwordCollider()
        {
            hitbox = GetComponent<Collider>();

            if (hitbox == null)
            {
                BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(1f, 0.2f, 2f);
                hitbox = boxCollider;

                Debug.Log($"⚠️ No collider found! Adding BoxCollider to {gameObject.name}.");
            }
            else
            {
                Debug.Log($"✅ Collider found on {gameObject.name}: {hitbox.GetType()}");
            }

            hitbox.isTrigger = false;
            hitbox.enabled = true;
        }

        // Debugging: Draw melee hitbox in Scene View
        private void OnDrawGizmosSelected()
        {
            if (attackPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(attackPoint.position, hitboxRadius);
            }
        }
    }
}
