using System;
using Core;
using Player.Base;
using UnityEngine;
using UnityEngine.UI;  // Import UI namespace

namespace Player.Components
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float currentHealth;
        public bool IsDead = false;
        public float MaxHealth { get; private set; }
        public bool IsInvulnerable { get; set; }

        public event Action<float, float> OnHealthChanged;
        public event Action OnDeath;

        private PlayerBase playerBase;
        private PlayerRagdoll playerRagdoll;

        private float lastDamageTime;
        private float regenDelay = 5f;
        private float regenSpeed = 20f;
        private bool isRegenerating = false;

        public float CurrentHealth => currentHealth;

        [Header("UI References")]
        [SerializeField] private Slider healthBar; // Reference to the health bar UI

        private void Awake()
        {
            playerBase = GetComponent<PlayerBase>();
            playerRagdoll = GetComponent<PlayerRagdoll>();

            if (playerBase == null)
            {
                Debug.LogError($"{name} is missing a PlayerBase component!");
                return;
            }

            MaxHealth = playerBase.maxHealth;
        }

        private void Start()
        {
            currentHealth = MaxHealth;
            lastDamageTime = Time.time;

            if (healthBar != null)
            {
                healthBar.maxValue = MaxHealth;
                healthBar.value = currentHealth;
            }

            OnHealthChanged += UpdateHealthBar; // Subscribe to event
        }

        private void UpdateHealthBar(float current, float max)
        {
            if (healthBar != null)
            {
                healthBar.value = current;
            }
        }

        private void Update()
        {
            HandleRegeneration();
        }

        public void TakeDamage(float damage)
        {
            if (IsDead || IsInvulnerable)
                return;

            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
            lastDamageTime = Time.time;

            Debug.Log($"🩸 Player took {damage} damage! Current HP: {currentHealth}/{MaxHealth}");

            OnHealthChanged?.Invoke(currentHealth, MaxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                isRegenerating = false;
            }
        }

        private void HandleRegeneration()
        {
            if (IsDead || currentHealth >= MaxHealth)
                return;

            if (Time.time - lastDamageTime >= regenDelay)
            {
                if (!isRegenerating)
                {
                    isRegenerating = true;
                    Debug.Log("🟢 Player started regenerating health!");
                }

                RegenerateHealth();
            }
        }

        private void RegenerateHealth()
        {
            currentHealth += regenSpeed * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);

            OnHealthChanged?.Invoke(currentHealth, MaxHealth);

            if (currentHealth >= MaxHealth)
            {
                isRegenerating = false;
                Debug.Log("✅ Health fully regenerated!");
            }
        }

        public void SetInvulnerability(float duration)
        {
            IsInvulnerable = true;
            Invoke(nameof(EndInvulnerability), duration);
        }

        private void EndInvulnerability()
        {
            IsInvulnerable = false;
        }

        private void Die()
        {
            if (IsDead)
                return;

            IsDead = true;
            isRegenerating = false;

            Debug.Log("🔥 Player has died!");

            if (playerRagdoll != null)
            {
                playerRagdoll.TriggerRagdoll();
            }

            OnDeath?.Invoke();
        }
    }
}
