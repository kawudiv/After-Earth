using UnityEngine;

namespace EnemyAI.Enemies.Melee.Type
{
    public class KriegEnemy : MeleeEnemy
    {
        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"{name} is a Soldier - he has nothing to lose!");

            enemyID = 1;
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
        }

        protected override void Update()
        {
            base.Update();
            CanFlee();
        }
    }
}
