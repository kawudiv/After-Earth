using EnemyAI.Base;
using EnemyAI.States.Melee;
using UnityEngine;

namespace EnemyAI.Enemies.Melee.Type
{
    public class BruteEnemy : MeleeEnemy
    {
        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"{name} is a Brute - slow but powerful!");
        }

        protected override void Start()
        {
            base.Start();
            maxHealth = 150f; // More HP than standard melee
            health = maxHealth;
            attackDamage = 20f; // Midway between Berserker & Assassin
            attackCooldown = 1.8f; // Slightly slower than normal melee
        }
        // protected override void Update()
        // {
        //     base.Update(); 
        //     SomeStuff(); 
        // }

        // private void SomeStuff()
        // {
        //     Debug.Log($"{name} is doing something unique!");
        //     // Additional custom logic specific to BruteEnemy
        // }
    }
}
