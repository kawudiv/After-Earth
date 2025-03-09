using UnityEngine;

namespace EnemyAI.Enemies.Ranged.Type
{
    public class SentryBotEnemy : RangedEnemy
    {
        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"{name} is a SentryBot - a basic ranged combat unit!");
        }

        protected override void Start()
        {
            base.Start();
            health = 120f; // Decent health for a standard ranged enemy
            armor = 4f; // Slightly armored but not tanky
            attackDamage = 10f; // Moderate damage
            attackCooldown = 2f; // Standard cooldown between shots
            criticalChance = 0.1f; // Occasional critical hits
            criticalDamage = 2f; // Double damage on crit
        }

        protected override void Update()
        {
            base.Update();
            SomeBehavior();
        }

        private void SomeBehavior()
        {
            Debug.Log($"{name} is scanning for targets...");
            // Additional custom logic for SentryBot
        }
    }
}
