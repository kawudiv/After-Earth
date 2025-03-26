using System;
using Core;
using Player.Base;
using UnityEngine;

namespace Player.Components
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private float currentHealth;
        public bool IsDead = false;
        public float MaxHealth { get; private set; }
        public bool IsInvulnerable { get; set; }

        public event Action<float, float> OnHealthChanged;
        public event Action OnDeath;

        private PlayerBase playerBase;
        private PlayerRagdoll playerRagdoll;
        private float regenerationRate = 0f;
        private float invulnerabilityDuration = 0f;
        private float regenerationDelay = 5f;
        private float lastDamageTime;

        public float CurrentHealth => currentHealth;

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
            InvokeRepeating(nameof(CheckRegeneration), 1f, 1f);
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
                Debug.Log("Player Died heeh");
                Die();
            }
        }

        private void CheckRegeneration()
        {
            if (IsDead || currentHealth <= 0 || Time.time - lastDamageTime < regenerationDelay)
                return;

            RegenerateHealth();
        }

        private void RegenerateHealth()
        {
            if (currentHealth >= MaxHealth)
                return;

            currentHealth += regenerationRate;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);

            Debug.Log(
                $"🟢 Player regenerates {regenerationRate} HP! Current HP: {currentHealth}/{MaxHealth}"
            );
            OnHealthChanged?.Invoke(currentHealth, MaxHealth);
        }

        public void SetRegeneration(float rate)
        {
            regenerationRate = rate;
        }

        public void SetInvulnerability(float duration)
        {
            IsInvulnerable = true;
            invulnerabilityDuration = duration;
            Invoke(nameof(EndInvulnerability), duration);
        }

        private void EndInvulnerability()
        {
            IsInvulnerable = false;
        }

        private void Die()
        {
            if (IsDead)
            {
                Debug.LogWarning($"[PlayerHealth] Die() called, but player is already dead.");
                return;
            }

            Debug.LogWarning("🔥 [PlayerHealth] Player is dying!");
            IsDead = true;
            CancelInvoke(nameof(CheckRegeneration));

            //playerBase.StateMachine.ChangeState(null);

            if (playerRagdoll != null)
            {
                Debug.Log($"[PlayerHealth] 🏋️ Triggering Ragdoll for {gameObject.name}!");
                playerRagdoll.TriggerRagdoll();
            }
            else
            {
                Debug.LogError($"[PlayerHealth] ❌ Cannot trigger ragdoll, playerRagdoll is NULL!");
            }

            Debug.Log("💀 Player has died!");
            OnDeath?.Invoke();
        }
    }
}
