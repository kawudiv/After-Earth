using System;
using EnemyAI.Base;
using UnityEngine;
using UnityEngine.UI; // Import UI namespace

namespace EnemyAI.Components
{
    public class EnemyHealth : MonoBehaviour, Core.IDamageable
    {
        [SerializeField]
        private float currentHealth;
        public float MaxHealth { get; private set; }

        public bool isDead = false;
        public event Action<float, float> OnHealthChanged;

        [SerializeField]
        private GameObject damageEffectPrefab;
        [SerializeField]
        private Slider healthBar; // ✅ Reference to the UI Slider

        private EnemyBase enemyBase;
        private EnemyRagdoll ragdoll;
        private float regen;

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
            MaxHealth = enemyBase.health; // ✅ Assign MaxHealth from EnemyBase (e.g., Krieg = 200)
            currentHealth = MaxHealth;
            regen = enemyBase.regeneration;

            if (healthBar != null)
            {
                SetHealthBar(MaxHealth, currentHealth); // ✅ Initialize health bar values
            }

            InvokeRepeating(nameof(RegenerateHealth), 1f, 1f);
        }

        public void TakeDamage(float damage)
        {
            if (isDead)
                return;

            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);

            Debug.Log($"🩸 {gameObject.name} took {damage} damage! Current HP: {currentHealth}/{MaxHealth}");

            OnHealthChanged?.Invoke(currentHealth, MaxHealth);

            if (healthBar != null)
            {
                healthBar.value = currentHealth; // ✅ Update slider value
            }

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
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);

            Debug.Log($"🟢 {gameObject.name} regenerates {regen} HP! Current HP: {currentHealth}/{MaxHealth}");

            OnHealthChanged?.Invoke(currentHealth, MaxHealth);

            if (healthBar != null)
            {
                healthBar.value = currentHealth; // ✅ Update UI slider
            }
        }

        private void Die()
        {
            if (isDead)
                return;

            isDead = true;
            CancelInvoke(nameof(RegenerateHealth));

            enemyBase.agent.enabled = false;
            ragdoll.TriggerRagdoll();

            Debug.Log($"💀 {gameObject.name} has died!");
            Destroy(gameObject, 5f);
        }

        /// <summary>
        /// ✅ Function to dynamically adjust the health bar based on max health.
        /// </summary>
        private void SetHealthBar(float maxHealth, float current)
        {
            if (healthBar != null)
            {
                healthBar.maxValue = maxHealth; // ✅ Set correct max HP (e.g., 200 for Krieg)
                healthBar.value = current;
            }
        }
    }
}
