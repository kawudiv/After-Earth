using System;
using EnemyAI.Base;
using UnityEngine;

namespace EnemyAI.Components
{
    public class EnemyHealth : MonoBehaviour, Core.IDamageable
    {
        [SerializeField] // ✅ Now visible in the Inspector
        private float currentHealth;
        public bool isDead = false;
        public event Action<float, float> OnHealthChanged;

        [SerializeField]
        private GameObject damageEffectPrefab;
        private EnemyBase enemyBase;
        private EnemyRagdoll ragdoll;

        public float CurrentHealth => currentHealth; // ✅ Read-only access

        private void Awake()
        {
           ragdoll = GetComponent<EnemyRagdoll>();
            enemyBase = GetComponentInParent<EnemyBase>();

            if (enemyBase == null)
            {
                Debug.LogError($"{name} is missing an EnemyBase component!");
                return; // Prevents further errors
            }  
        }
        private void Start()
        {
            currentHealth = enemyBase.health;
        }

        public void TakeDamage(float damage)
        {
            if (isDead)
                return; // ✅ Ignore further damage if already dead

            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, enemyBase.health); // ✅ Prevent negative HP

            Debug.Log($"🩸 {gameObject.name} took {damage} damage! Current HP: {currentHealth}");

            // ✅ Trigger the health update event
            OnHealthChanged?.Invoke(currentHealth, enemyBase.health);

            // ✅ Show damage effect (blood splatter)
            if (damageEffectPrefab != null)
            {
                Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);
            }

            // ✅ If HP reaches 0, trigger full death ragdoll
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                Debug.Log($"😡 {gameObject.name} flinches from damage!");
                // ✅ Play a stagger animation if needed (requires Animator)
                Animator animator = GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger("Hit");
                }
            }
        }

        private void Die()
        {
            if (isDead)
                return; // ✅ Prevent multiple calls

            isDead = true;
            ragdoll.TriggerRagdoll(); // ✅ Fully activate ragdoll on death

            Debug.Log($"💀 {gameObject.name} has died!");

            Destroy(gameObject, 5f); // ✅ Remove enemy after 5 sec (optional)
        }
    }
}
