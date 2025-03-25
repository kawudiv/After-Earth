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
       // private HashSet<IDamageable> damagedEnemies = new HashSet<IDamageable>();

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
                hitbox.isTrigger = true; // ✅ Enable trigger to detect enemy hits
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
                return; // ✅ Ignore if the hitbox is disabled or not in attack mode

            if (((1 << other.gameObject.layer) & entityLayers) == 0)
            {
                return; // Ignore non-enemy collisions
            }

            Debug.Log($"🎯 Hit detected on: {other.name}");

            Transform root = other.transform.root;

            if (root.GetComponentInChildren<IDamageable>() is IDamageable damageable)
            {
                if (!damagedEnemies.Contains(damageable))
                {
                    Debug.Log($"⚔️ {weaponName} dealing {damage} damage to {root.name}");
                    damageable.TakeDamage(damage);
                    damagedEnemies.Add(damageable);
                }
            }
            else
            {
                Debug.Log($"❌ {root.name} does NOT have IDamageable (even in children)!");
            }

            // ✅ Handle ragdoll and physics impact
            MeleeImpact(other);
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

            // ✅ Spawn hit effect
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
                hitbox.convex = true; // ✅ Required for trigger detection
                Debug.Log($"⚠️ No MeshCollider found! Adding MeshCollider to {gameObject.name}.");
            }
            else
            {
                Debug.Log($"✅ MeshCollider found on {gameObject.name}");
            }

            hitbox.enabled = true;
            hitbox.isTrigger = false; // ✅ Enable for pickup but prevent damage while on ground
        }

        public void OnPickup()
        {
            if (hitbox != null)
            {
                hitbox.enabled = false; // ✅ Disable collider when equipped
            }
            Debug.Log($"[MeleeWeapon] Picked up, collider disabled on {gameObject.name}");
        }

        public void OnDrop()
        {
            if (hitbox != null)
            {
                hitbox.enabled = true;
                hitbox.isTrigger = false; // ✅ Enable pickup but prevent damage
            }
            Debug.Log(
                $"[MeleeWeapon] Dropped, collider enabled but non-damaging on {gameObject.name}"
            );
        }
    }
}
