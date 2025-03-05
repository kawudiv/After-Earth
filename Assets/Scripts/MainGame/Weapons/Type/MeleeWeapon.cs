using Core;
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
            foreach (Collider enemy in hitEnemies)
            {
                // ✅ Apply damage inside Attack()
                if (enemy.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damage);
                    Debug.Log($"{weaponName} hit {enemy.name} for {damage} damage!");

                    // ✅ Spawn hit effect at impact position
                    if (hitEffectPrefab != null)
                    {
                        Instantiate(hitEffectPrefab, enemy.transform.position, Quaternion.identity);
                    }
                }

                // ✅ Call SwordImpact() to handle physics separately
                SwordImpact(enemy);
            }
        }

        private void SwordImpact(Collider enemy)
        {
            // ✅ Apply force to ragdoll/enemy rigidbody
            if (enemy.TryGetComponent(out Rigidbody rb))
            {
                Vector3 forceDirection =
                    (enemy.transform.position - attackPoint.position).normalized * impactForce;
                rb.AddForce(forceDirection, ForceMode.Impulse);
                Debug.Log(
                    $"Applied {impactForce} force to {enemy.name}, making them stagger/slash away!"
                );
            }

            // ✅ Spawn hit effect at impact position
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, enemy.transform.position, Quaternion.identity);
            }
        }

        // ✅ Configure sword collider
        private void ConfigureSwordCollider()
        {
            hitbox = GetComponent<Collider>();

            if (hitbox == null)
            {
                BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(1f, 0.2f, 2f); // Adjust for sword shape
                hitbox = boxCollider;
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
