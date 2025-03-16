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
        private Transform aimPrefab; // Reference to your aim prefab

        [Header("Bullet Settings")]
        [SerializeField]
        protected float bulletForce = 10f; // Force applied to ragdoll

        [SerializeField]
        protected int bulletCount = 1; // Default 1 bullet per shot

        [SerializeField]
        protected float spreadAngle = 5f; // Shotgun spread angle

        [Header("Weapon Layers")]
        [SerializeField]
        protected LayerMask enemyLayers;

        [SerializeField]
        protected LayerMask environmentLayers;

        [Header("Weapon Effects")]
        [SerializeField]
        private GameObject impactEffectPrefab;

        [SerializeField]
        private GameObject hitEffectPrefab; // Enemy hit effect

        [Header("Left Hand IK Settings")]
        public float leftHandIKWeight;
        public Vector3 leftHandIKPosition;
        public Quaternion leftHandIKRotation;
        public Vector3 leftHandIKHintPosition;

        [Header("Right Hand IK Settings")] // ✅ NEW: Right Hand IK Settings
        public float rightHandIKWeight;
        public Vector3 rightHandIKPosition;
        public Quaternion rightHandIKRotation;
        public Vector3 rightHandIKHintPosition;

        [Header("Tracking Weights")]
        public float bodyTrackingWeight;
        public float gunTrackingWeight;

        [HideInInspector]
        public int rangeID;

        // Store last shot directions for Gizmos
        private Vector3[] lastShotDirections;

        protected override void Awake()
        {
            base.Awake();
            weaponTypeID = rangeID;

            if (attackPoint == null)
            {
                Debug.LogWarning($"{weaponName} has no attackPoint assigned!");
            }

            if (aimPrefab == null)
            {
                Debug.LogWarning($"{weaponName} has no aimPrefab assigned!");
            }
        }

        public override void Attack()
        {
            Debug.Log($"{weaponName} fires {bulletCount} ray(s)!");

            if (attackPoint == null || aimPrefab == null)
            {
                Debug.LogWarning($"{weaponName} has no attackPoint or aimPrefab assigned!");
                return;
            }

            // Store shot directions for Gizmos
            lastShotDirections = new Vector3[bulletCount];

            for (int i = 0; i < bulletCount; i++)
            {
                Vector3 shootDirection;

                if (i == 0)
                {
                    // First shot is always accurate and aligned with the aim prefab
                    shootDirection = (aimPrefab.position - attackPoint.position).normalized;
                }
                else
                {
                    // Subsequent shots are dispersed around the first shot
                    shootDirection = GetSpreadDirection((aimPrefab.position - attackPoint.position).normalized);
                }

                lastShotDirections[i] = shootDirection; // Store direction for Gizmos
                FireRaycast(shootDirection);
            }
        }

        private void FireRaycast(Vector3 direction)
        {
            Debug.Log($"🔫 {weaponName} firing raycast...");

            RaycastHit hit;
            if (
                Physics.Raycast(
                    attackPoint.position,
                    direction,
                    out hit,
                    Mathf.Infinity,
                    enemyLayers | environmentLayers
                )
            )
            {
                Debug.Log($"✅ Raycast hit {hit.collider.gameObject.name} at {hit.point}");

                // Check if the hit object is an enemy
                if (((1 << hit.collider.gameObject.layer) & enemyLayers) != 0)
                {
                    Debug.Log($"💥 {weaponName} hit an enemy: {hit.collider.gameObject.name}");

                    IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(damage);
                    }

                    // Spawn enemy hit effect
                    if (hitEffectPrefab != null)
                    {
                        Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
                    }

                    // Apply force to ragdoll if the enemy has one
                    RangedImpact(hit.collider, hit.point);
                }
                else if (((1 << hit.collider.gameObject.layer) & environmentLayers) != 0)
                {
                    Debug.Log("🌲 Raycast hit the environment!");

                    // Spawn impact effect on environment
                    if (impactEffectPrefab != null)
                    {
                        Instantiate(impactEffectPrefab, hit.point, Quaternion.identity);
                    }
                }
            }
            else
            {
                Debug.Log("❌ Raycast didn't hit anything!");
            }
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
            Quaternion spreadRotation = Quaternion.Euler(spreadX, spreadY, 0);
            return spreadRotation * baseDirection;
        }

        /// <summary>
        /// Draw Gizmos to visualize raycast shots.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (attackPoint == null || lastShotDirections == null)
                return;

            for (int i = 0; i < lastShotDirections.Length; i++)
            {
                Vector3 shotDirection = lastShotDirections[i];
                Gizmos.color = (i == 0) ? Color.red : Color.blue; // Main shot = red, spread shots = blue
                Gizmos.DrawRay(attackPoint.position, shotDirection * 100f); // Extend the ray for visualization
            }
        }
    }
}