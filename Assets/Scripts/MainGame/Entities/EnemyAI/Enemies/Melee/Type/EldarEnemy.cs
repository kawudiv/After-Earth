using UnityEngine;

namespace EnemyAI.Enemies.Melee.Type
{
    public class EldarEnemy : MeleeEnemy
    {
        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"{name} is an Eldar Assassin - deadly, agile, and strategic!");

            enemyID = 4;
            patrolSpeed = 2.5f;  // ✅ Faster movement speed
            chaseSpeed = 6.0f;   // ⚡ Extremely quick
            attackRange = 1.8f;  // ✅ Close-quarters combat
            detectionRange = 12f; // 🔥 Good awareness

            health = 120f;   // ❌ Low HP, relies on evasion
            armor = 2f;      // ❌ Low armor, weak to heavy attacks
            regeneration = 5f; // ✅ Regenerates quickly when out of combat
            attackDamage = 35f; // 💀 High assassination damage
            attackCooldown = 1.2f; // ⚡ Quick attack speed

            canFlee = true; // ✅ Assassins retreat when wounded
            Debug.Log($"{name} uses hit-and-run tactics!");
        }

        protected override void Update()
        {
            base.Update();
            CanFlee();
        }
    }
}
