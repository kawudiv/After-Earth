using System;
using System.Collections.Generic;
using Core;
using EnemyAI.Components;
using UnityEngine;
using Weapons.Base;

namespace Weapons.Types
{
    public class MeleeWeapon : WeaponBase
    {
        public Transform attackPoint;

        [Header("Hitbox Settings")]
        [SerializeField]
        protected LayerMask entityLayers;

        [Header("Physics Settings")]
        [SerializeField]
        protected float impactForce; // Force applied to ragdoll

        [Header("Weapon Effects")]
        [SerializeField]
        protected GameObject impactEffectPrefab; // Slash effect

        [SerializeField]
        protected GameObject hitEffectPrefab; // Enemy hit effect

        [HideInInspector]
        public int meleeID;
        private MeshCollider hitbox; // Mesh Collider for hit detection
        private Dictionary<Collider, IDamageable> damageableCache =
            new Dictionary<Collider, IDamageable>();

        protected override void Awake()
        {
            base.Awake();
            weaponTypeID = meleeID;
            ConfigureMeleeCollider();
        }

        public override void Attack()
        {
            Debug.Log($"{weaponName} is attacking!");
            ResetHitRecords();
        }

        internal void EnableWeaponCollider()
        {
            if (hitbox != null)
            {
                hitbox.enabled = true;
                hitbox.isTrigger = true;
                Debug.Log($"[MeleeWeapon] Enabled hitbox for {gameObject.name}");
            }
        }

        internal void DisableWeaponCollider()
        {
            if (hitbox != null)
            {
                hitbox.enabled = false;
                Debug.Log($"[MeleeWeapon] Disabled hitbox for {gameObject.name}");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!hitbox.enabled || !hitbox.isTrigger)
                return;

            if (((1 << other.gameObject.layer) & entityLayers) == 0)
            {
                return; // Ignore non-enemy collisions
            }

            Debug.Log($"🎯 Hit detected on: {other.name}");

            // Get or cache the IDamageable component
            if (!damageableCache.TryGetValue(other, out IDamageable damageable))
            {
                damageable = other.GetComponentInParent<IDamageable>();
                if (damageable != null)
                {
                    damageableCache[other] = damageable;
                }
            }

            if (damageable != null)
            {
                if (!damagedEnemies.Contains(damageable))
                {
                    Debug.Log(
                        $"⚔️ {weaponName} dealing {damage} damage to {other.transform.root.name}"
                    );
                    damageable.TakeDamage(damage);
                    damagedEnemies.Add(damageable);

                    // Handle impact effects
                    MeleeImpact(other);
                }
            }
            else
            {
                Debug.Log($"❌ {other.transform.root.name} does NOT have IDamageable!");
            }
        }

        private void MeleeImpact(Collider entity)
        {
            EnemyHealth enemyHealth = entity.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null && enemyHealth.isDead)
            {
                EnemyRagdoll ragdoll = entity.GetComponentInParent<EnemyRagdoll>();
                if (ragdoll != null)
                {
                    Vector3 hitPoint = entity.ClosestPoint(attackPoint.position);
                    Vector3 forceDirection =
                        (entity.transform.position - attackPoint.position).normalized * impactForce;

                    ragdoll.TriggerRagdoll();
                    ragdoll.ApplyForceToRagdoll(hitPoint, forceDirection);
                }
            }
            else if (entity.TryGetComponent(out Rigidbody rb))
            {
                Vector3 forceDirection =
                    (entity.transform.position - attackPoint.position).normalized * impactForce;
                rb.AddForce(forceDirection, ForceMode.Impulse);
                Debug.Log($"💥 Knockback applied to {entity.name}!");
            }

            // Spawn hit effect
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, entity.transform.position, Quaternion.identity);
            }
        }

        private void ConfigureMeleeCollider()
        {
            hitbox = GetComponent<MeshCollider>();

            if (hitbox == null)
            {
                hitbox = gameObject.AddComponent<MeshCollider>();
                hitbox.convex = true;
                Debug.Log($"⚠️ No MeshCollider found! Adding MeshCollider to {gameObject.name}.");
            }
            else
            {
                Debug.Log($"✅ MeshCollider found on {gameObject.name}");
            }

            hitbox.enabled = true;
            hitbox.isTrigger = false;
        }

        public void OnPickup()
        {
            if (hitbox != null)
            {
                hitbox.enabled = false;
            }
            Debug.Log($"[MeleeWeapon] Picked up, collider disabled on {gameObject.name}");
        }

        public void OnDrop()
        {
            if (hitbox != null)
            {
                hitbox.enabled = true;
                hitbox.isTrigger = false;
            }
            Debug.Log(
                $"[MeleeWeapon] Dropped, collider enabled but non-damaging on {gameObject.name}"
            );
        }

        private void OnDestroy()
        {
            damageableCache.Clear();
        }
    }
}
