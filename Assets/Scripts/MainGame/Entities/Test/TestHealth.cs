using System; // ✅ Required for events
using UnityEngine;

namespace EnemyAi.Test
{
    public class EnemyHealth : MonoBehaviour, Core.IDamageable
    {
        public float maxHealth = 100f;

        [SerializeField] // ✅ Now visible in the Inspector
        private float currentHealth;
        private TestRagdoll ragdoll;
        public bool isDead = false; // ✅ Prevent multiple death triggers

        // ✅ Event: Notifies when HP changes (for UI, sound, etc.)
        public event Action<float, float> OnHealthChanged;

        // ✅ Optional: Blood effect on damage
        [SerializeField]
        private GameObject damageEffectPrefab;

        void Awake()
        {
            currentHealth = maxHealth;
            ragdoll = GetComponent<TestRagdoll>();
        }

        // ✅ This method is required by IDamageable
        public void TakeDamage(float damage)
        {
            if (isDead)
                return; // ✅ Ignore further damage if already dead

            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ✅ Prevent negative HP

            Debug.Log($"🩸 {gameObject.name} took {damage} damage! Current HP: {currentHealth}");

            // ✅ Trigger the health update event
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

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
