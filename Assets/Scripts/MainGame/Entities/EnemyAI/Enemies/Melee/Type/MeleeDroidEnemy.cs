using UnityEngine;

namespace EnemyAI.Enemies.Melee.Type
{
    public class MeleeDroidEnemy : MeleeEnemy
    {
        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"{name} is a Melee Combat Droid - relentless and mechanical!");

            enemyID = 3;
            patrolSpeed = 1.5f; // ✅ More efficient movement
            chaseSpeed = 4.0f; // ✅ Fast but methodical
            attackRange = 2f;
            detectionRange = 15f; // ✅ High detection range due to sensors

            health = 250f; // ✅ Higher durability
            armor = 8f; // ✅ Heavy armor
            regeneration = 0f; // ❌ No health regeneration (Droids don't heal)
            attackDamage = 22f;
            attackCooldown = 1.5f; // ⚡ Balanced attack speed

            canFlee = false; // ❌ Droids do NOT flee
            Debug.Log($"{name} is a programmed killing machine. No retreat, no fear.");
        }

        protected override void Update()
        {
            base.Update();
            // ❌ No fleeing logic, as it's a droid
        }
    }
}
