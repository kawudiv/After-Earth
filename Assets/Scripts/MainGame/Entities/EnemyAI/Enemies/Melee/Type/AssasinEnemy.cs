using UnityEngine;

namespace EnemyAI.Enemies.Melee.Type
{
    public class AssassinEnemy : MeleeEnemy
    {
        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"{name} is an Assassin - fast but fragile!");
        }

        protected override void Start()
        {
            base.Start();
            maxHealth = 60f; // Lower health
            patrolSpeed = 3.5f;
            chaseSpeed = 6f; // Very fast
            attackRange = 1.5f;
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
