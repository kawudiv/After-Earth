using UnityEngine;

namespace EnemyAI.Enemies.Boss.Type
{
    public class OverlordBoss : BossEnemy
    {
        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"{name} is the Overlord, the most powerful boss!");
        }

        protected override void Start()
        {
            base.Start();
            maxHealth = 300f; // Overlord has more health
            health = maxHealth;
            attackDamage = 40f; // Stronger attacks
            attackCooldown = 2.5f; // Slower attacks
            armor = 15f; // High defense
        }

        public override void PerformSpecialAttack()
        {
            Debug.Log($"{name} unleashes a devastating energy blast!");
            // Implement special attack logic, e.g., area-of-effect damage
        }

        protected override void Update()
        {
            base.Update(); // Runs the default EnemyBase update logic
            SomeStuff(); // Custom behavior for the Overlord boss
        }

        private void SomeStuff()
        {
            Debug.Log($"{name} is doing something unique!");
            // Additional custom logic specific to OverlordBoss
        }
    }
}
