using UnityEngine;

namespace EnemyAI.Enemies.Melee.Type
{
    public class BruteEnemy : MeleeEnemy
    {
        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"{name} is a Brute - slow but powerful!");

            enemyID = 0;
            patrolSpeed = 2f;
            chaseSpeed = 4f;
            attackRange = 2f;
            detectionRange = 10f;

            health = 150f; // More HP than standard melee
            attackDamage = 20f; // Midway between Berserker & Assassin
            attackCooldown = 1.8f; // Slightly slower than normal melee
        }
    }
}
