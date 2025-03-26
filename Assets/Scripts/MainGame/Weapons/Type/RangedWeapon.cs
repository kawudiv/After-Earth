using System.Collections.Generic;
using Core;
using EnemyAI.Components;
using UnityEngine;
using Weapons.Base;

namespace Weapons.Types
{
    public class RangedWeapon : WeaponBase
    {
        public Transform attackPoint;

        [Header("Aim Prefab")]
        [SerializeField]
        private Transform aimPrefab;

        [Header("Bullet Settings")]
        [SerializeField]
        protected float bulletForce = 10f;

        [SerializeField]
        protected int bulletCount = 1;

        [SerializeField]
        protected float spreadAngle = 5f;

        [Header("Weapon Layers")]
        [SerializeField]
        protected LayerMask enemyLayers;

        [SerializeField]
        protected LayerMask environmentLayers;

        [Header("Weapon Effects")]
        [SerializeField]
        private GameObject impactEffectPrefab;

        [SerializeField]
        private GameObject hitEffectPrefab;

        [Header("IK Settings")]
        public float leftHandIKWeight;
        public Vector3 leftHandIKPosition;
        public Quaternion leftHandIKRotation;
        public Vector3 leftHandIKHintPosition;
        public float rightHandIKWeight;
        public Vector3 rightHandIKPosition;
        public Quaternion rightHandIKRotation;
        public Vector3 rightHandIKHintPosition;
        public float bodyTrackingWeight;
        public float gunTrackingWeight;

        [HideInInspector]
        public int rangeID;

        private Vector3[] lastShotDirections;
        private Dictionary<Collider, IDamageable> damageableCache =
            new Dictionary<Collider, IDamageable>();
        private Dictionary<Collider, HashSet<int>> pelletHitsThisAttack =
            new Dictionary<Collider, HashSet<int>>();
        private int currentPelletID = 0;

        protected override void Awake()
        {
            base.Awake();
            weaponTypeID = rangeID;

            if (attackPoint == null)
                Debug.LogWarning($"{weaponName} has no attackPoint assigned!");
            if (aimPrefab == null)
                Debug.LogWarning($"{weaponName} has no aimPrefab assigned!");
        }

        public override void Attack()
        {
            Debug.Log($"{weaponName} fires {bulletCount} ray(s)!");
            ResetHitRecords();
            pelletHitsThisAttack.Clear();
            currentPelletID = 0;

            if (attackPoint == null || aimPrefab == null)
            {
                Debug.LogWarning($"{weaponName} has no attackPoint or aimPrefab assigned!");
                return;
            }

            lastShotDirections = new Vector3[bulletCount];

            for (int i = 0; i < bulletCount; i++)
            {
                currentPelletID = i; // Track which pellet we're firing
                Vector3 shootDirection =
                    i == 0
                        ? (aimPrefab.position - attackPoint.position).normalized
                        : GetSpreadDirection(
                            (aimPrefab.position - attackPoint.position).normalized
                        );

                lastShotDirections[i] = shootDirection;
                FireRaycast(shootDirection);
            }
        }

        private void FireRaycast(Vector3 direction)
        {
            Debug.Log($"🔫 {weaponName} firing raycast...");

            if (
                Physics.Raycast(
                    attackPoint.position,
                    direction,
                    out RaycastHit hit,
                    Mathf.Infinity,
                    enemyLayers | environmentLayers
                )
            )
            {
                Debug.Log($"✅ Raycast hit {hit.collider.gameObject.name} at {hit.point}");

                // Handle enemy hit
                if (((1 << hit.collider.gameObject.layer) & enemyLayers) != 0)
                {
                    HandleEnemyHit(hit.collider, hit.point);
                }
                // Handle environment hit
                else if (((1 << hit.collider.gameObject.layer) & environmentLayers) != 0)
                {
                    HandleEnvironmentHit(hit.point);
                }
            }
            else
            {
                Debug.Log("❌ Raycast didn't hit anything!");
            }
        }

        private void HandleEnemyHit(Collider enemyCollider, Vector3 hitPoint)
        {
            Debug.Log($"💥 {weaponName} hit an enemy: {enemyCollider.gameObject.name}");

            // Get or cache the IDamageable component
            if (!damageableCache.TryGetValue(enemyCollider, out IDamageable damageable))
            {
                damageable = enemyCollider.GetComponentInParent<IDamageable>();
                if (damageable != null)
                    damageableCache[enemyCollider] = damageable;
            }

            if (damageable != null)
            {
                // Check if this specific pellet has already hit this enemy
                if (!pelletHitsThisAttack.TryGetValue(enemyCollider, out HashSet<int> hitPelletIDs))
                {
                    hitPelletIDs = new HashSet<int>();
                    pelletHitsThisAttack[enemyCollider] = hitPelletIDs;
                }

                if (!hitPelletIDs.Contains(currentPelletID))
                {
                    hitPelletIDs.Add(currentPelletID);
                    damageable.TakeDamage(damage);

                    if (hitEffectPrefab != null)
                        Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);

                    RangedImpact(enemyCollider, hitPoint);
                }
                else
                {
                    Debug.Log($"🚫 Pellet {currentPelletID} already hit this enemy");
                }
            }
        }

        protected override void ResetHitRecords()
        {
            base.ResetHitRecords();
            pelletHitsThisAttack.Clear();
        }

        private void HandleEnvironmentHit(Vector3 hitPoint)
        {
            Debug.Log("🌲 Raycast hit the environment!");
            if (impactEffectPrefab != null)
                Instantiate(impactEffectPrefab, hitPoint, Quaternion.identity);
        }

        private void RangedImpact(Collider enemy, Vector3 hitPoint)
        {
            EnemyHealth enemyHealth = enemy.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null && enemyHealth.isDead)
            {
                EnemyRagdoll ragdoll = enemy.GetComponentInParent<EnemyRagdoll>();
                if (ragdoll != null)
                {
                    Vector3 forceDirection =
                        (enemy.transform.position - attackPoint.position).normalized * bulletForce;
                    ragdoll.TriggerRagdoll();
                    ragdoll.ApplyForceToRagdoll(hitPoint, forceDirection);
                }
            }
            else if (enemy.TryGetComponent(out Rigidbody rb))
            {
                Vector3 forceDirection =
                    (enemy.transform.position - attackPoint.position).normalized * bulletForce;
                rb.AddForceAtPosition(forceDirection, hitPoint, ForceMode.Impulse);
                Debug.Log($"💥 Knockback applied to {enemy.name}!");
            }
        }

        private Vector3 GetSpreadDirection(Vector3 baseDirection)
        {
            float spreadX = Random.Range(-spreadAngle, spreadAngle);
            float spreadY = Random.Range(-spreadAngle, spreadAngle);
            return Quaternion.Euler(spreadX, spreadY, 0) * baseDirection;
        }

        private void OnDrawGizmos()
        {
            if (attackPoint == null || lastShotDirections == null)
                return;

            for (int i = 0; i < lastShotDirections.Length; i++)
            {
                Gizmos.color = (i == 0) ? Color.red : Color.blue;
                Gizmos.DrawRay(attackPoint.position, lastShotDirections[i] * 100f);
            }
        }

        private void OnDestroy()
        {
            damageableCache.Clear();
        }
    }
}
