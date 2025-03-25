using UnityEngine;

namespace EnemyAI.Enemies.Melee.Type
{
    public class BerserkerEnemy : MeleeEnemy
    {
        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"{name} is a Berserker - relentless and fearless!");

            enemyID = 2;
            patrolSpeed = 1.2f;
            chaseSpeed = 4.5f; // ✅ Faster than KriegEnemy
            attackRange = 1.5f;
            detectionRange = 12f; // ✅ More aggressive detection range

            health = 180f;
            armor = 3f;
            regeneration = 0f; // ❌ No health regeneration
            attackDamage = 25f; // ✅ Higher damage than KriegEnemy
            attackCooldown = 1.2f; // ✅ Attacks faster

            canFlee = false; // ❌ This enemy will NEVER flee
            Debug.Log($"{name} is fearless and will fight to the death!");
        }

        protected override void Update()
        {
            base.Update();
            // ❌ No need to call CanFlee() because Berserker never flees
        }
    }
}
