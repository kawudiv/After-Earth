using System;
using EnemyAI.Base;
using UnityEngine;

namespace EnemyAI.Components
{
    public class EnemyHealth : MonoBehaviour, Core.IDamageable
    {
        [SerializeField]
        private float currentHealth;
        public bool isDead = false;
        public event Action<float, float> OnHealthChanged;

        [SerializeField]
        private GameObject damageEffectPrefab;

        private EnemyBase enemyBase;
        private EnemyRagdoll ragdoll;
        private float regen; // ✅ Added regeneration variable

        public float CurrentHealth => currentHealth;

        private void Awake()
        {
            ragdoll = GetComponent<EnemyRagdoll>();
            enemyBase = GetComponentInParent<EnemyBase>();

            if (enemyBase == null)
            {
                Debug.LogError($"{name} is missing an EnemyBase component!");
                return;
            }
        }

        private void Start()
        {
            currentHealth = enemyBase.health;
            regen = enemyBase.regeneration;
            InvokeRepeating(nameof(RegenerateHealth), 1f, 1f); // ✅ Start health regeneration every second
        }

        public void TakeDamage(float damage)
        {
            if (isDead)
                return;

            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, enemyBase.health);

            Debug.Log($"🩸 {gameObject.name} took {damage} damage! Current HP: {currentHealth}");

            OnHealthChanged?.Invoke(currentHealth, enemyBase.health);

            if (damageEffectPrefab != null)
            {
                Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);
            }

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                Debug.Log($"😡 {gameObject.name} flinches from damage!");
                Animator animator = GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger("Hit");
                }
            }
        }

        private void RegenerateHealth()
        {
            if (isDead || currentHealth <= 0 || regen <= 0)
                return;

            currentHealth += regen;
            currentHealth = Mathf.Clamp(currentHealth, 0, enemyBase.health);

            Debug.Log($"🟢 {gameObject.name} regenerates {regen} HP! Current HP: {currentHealth}");

            OnHealthChanged?.Invoke(currentHealth, enemyBase.health);
        }

        private void Die()
        {
            if (isDead)
                return;

            isDead = true;
            CancelInvoke(nameof(RegenerateHealth)); // ✅ Stop regeneration on death
            ragdoll.TriggerRagdoll();

            Debug.Log($"💀 {gameObject.name} has died!");
            Destroy(gameObject, 5f);
        }
    }
}
