using UnityEngine;

namespace Player.Components
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        private float currentHealth;
        public float armor = 5f;
        public bool IsInvulnerable { get; set; } = false; // ✅ Added invulnerability flag

        public delegate void HealthChanged(float current, float max);
        public event HealthChanged OnHealthChanged;
        public event System.Action OnDeath;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (IsInvulnerable)
            {
                Debug.Log("Player is invulnerable! No damage taken.");
                return; // ✅ Prevent damage if invulnerable
            }

            float damageTaken = Mathf.Max(damage - armor, 0); // Prevent negative damage
            currentHealth -= damageTaken;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDeath?.Invoke();
            }

            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public void Heal(float amount)
        {
            currentHealth += amount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public float GetHealthPercentage()
        {
            return currentHealth / maxHealth;
        }
    }
}
