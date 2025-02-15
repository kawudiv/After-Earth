using UnityEngine;

namespace EnemyAI.Components
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        private float currentHealth;

        public delegate void HealthChanged(float current, float max);
        public event HealthChanged OnHealthChanged;
        public event System.Action OnDeath;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            OnDeath?.Invoke();
            Destroy(gameObject, 2f); // Delay destruction for effects
        }

        public void Heal(float amount)
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public float GetHealthPercentage()
        {
            return currentHealth / maxHealth;
        }
    }
}
