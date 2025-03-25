using UnityEngine;

namespace EnemyAI.Enemies.Melee.Type
{
    public class KriegEnemy : MeleeEnemy
    {
        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"{name} is a Brute - slow but powerful!");

            patrolSpeed = 1f;
            chaseSpeed = 3f;
            attackRange = 2f;
            detectionRange = 10f;

            health = 200f;
            armor = 5f;
            regeneration = 3f;
            attackDamage = 20f;
            attackCooldown = 1.8f;

            canFlee = true;
            Debug.Log($"{name} has flee ability enabled.");
        }

        protected override void CheckHealthAndReact()
        {
            base.CheckHealthAndReact(); // ✅ Already handles fleeing logic in MeleeEnemy

            // ✅ Only add extra behavior if needed
            Debug.Log($"{name} (KriegEnemy) Health Check: {enemyHealth.CurrentHealth}");
        }
    }
}
