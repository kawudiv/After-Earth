using UnityEngine;

namespace EnemyAI.Enemies.Melee.Type
{
    public class BerserkerEnemy : MeleeEnemy
    {
        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"{name} is a Berserker - strong but slow!");
        }

        protected override void Start()
        {
            base.Start();
            maxHealth = 200f; // High health
            health = maxHealth;
            patrolSpeed = 2f;
            chaseSpeed = 3.5f; // Slower speed
        }

        protected override void Update()
        {
            base.Update();
            AdjustAttackSpeed();
        }

        private void AdjustAttackSpeed()
        {
            float healthPercentage = health / maxHealth;
            float attackSpeedMultiplier = 1f + (1f - healthPercentage); // Faster at low health
            animator.SetFloat("AttackSpeed", attackSpeedMultiplier);
        }
    }
}
