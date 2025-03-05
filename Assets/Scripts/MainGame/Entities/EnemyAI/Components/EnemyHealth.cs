using EnemyAI.Base;
using UnityEngine;

namespace EnemyAI.Components
{
    public class EnemyHealth : MonoBehaviour
    {
        private float currentHealth;
        private bool isDead = false;

        public delegate void HealthChanged(float current, float max);
        public event HealthChanged OnHealthChanged;
        public event System.Action OnDeath;

        private EnemyBase enemyBase;
        private EnemyRagdoll enemyRagdoll;

        private void Awake()
        {
            enemyRagdoll = GetComponent<EnemyRagdoll>();
            enemyBase = GetComponent<EnemyBase>();

            if (enemyBase == null)
            {
                Debug.LogError($"{name} is missing an EnemyBase component!");
                return; // Prevents further errors
            }

            currentHealth = enemyBase.maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (isDead)
                return; // Ignore damage if already dead

            currentHealth = Mathf.Max(currentHealth - damage, 0);
            OnHealthChanged?.Invoke(currentHealth, enemyBase.maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Heal(float amount)
        {
            if (currentHealth >= enemyBase.maxHealth)
                return; // Already at full health

            currentHealth = Mathf.Min(currentHealth + amount, enemyBase.maxHealth);
            OnHealthChanged?.Invoke(currentHealth, enemyBase.maxHealth);
        }

        private void Die()
        {
            enemyRagdoll.ActivateRagdoll();
            if (isDead)
                return; // Prevent multiple death calls
            isDead = true;

            OnDeath?.Invoke();
            Destroy(gameObject, 2f);
        }

        public float GetHealthPercentage()
        {
            return currentHealth / enemyBase.maxHealth;
        }
    }
}
