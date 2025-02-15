using UnityEngine;

namespace EnemyAI.Enemies.Ranged.Type
{
    public class SniperEnemy : RangedEnemy
    {
        public float chargeTime = 2f; // Time before firing a powerful shot

        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"{name} initialized as a Sniper - precise and deadly from long range!");
        }

        protected override void Start()
        {
            base.Start();
            maxHealth = 80f; // Lower health due to sniper role
            health = maxHealth;
            armor = 3f; // Light armor for mobility
            attackDamage = 40f; // High damage for long-range shots
            attackCooldown = 3f; // Longer cooldown due to charge mechanic
            criticalChance = 0.3f; // Higher crit chance for precision shots
            criticalDamage = 2.5f; // More lethal shots
        }

        public override void FireProjectile()
        {
            Debug.Log($"{name} is charging a powerful sniper shot...");
            Invoke(nameof(FireChargedShot), chargeTime); // Delays shot for charge effect
        }

        private void FireChargedShot()
        {
            if (projectilePrefab && firePoint)
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                Debug.Log($"{name} fired a charged sniper shot!");
            }
            else
            {
                Debug.LogWarning($"{name} is missing projectilePrefab or firePoint!");
            }
        }

        protected override void Update()
        {
            base.Update();
            SomeStuff(); 
        }

        private void SomeStuff()
        {
            Debug.Log($"{name} is scanning for targets...");
            // Additional custom logic for SniperEnemy
        }
    }
}
