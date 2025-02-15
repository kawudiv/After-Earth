using EnemyAI.Base;
using EnemyAI.States.Ranged;
using UnityEngine;

namespace EnemyAI.Enemies.Ranged
{
    public class RangedEnemy : EnemyBase
    {
        public GameObject projectilePrefab;
        public Transform firePoint;

        private RangedChaseState rangedChaseState;
        private RangedAttackState rangedAttackState;

        public override State ChaseState => rangedChaseState;
        public override State AttackState => rangedAttackState;

        protected override void Awake()
        {
            base.Awake();
            rangedChaseState = new RangedChaseState(this, stateMachine);
            rangedAttackState = new RangedAttackState(this, stateMachine);
            stateMachine.Initialize(rangedChaseState);
        }

        public virtual void FireProjectile()
        {
            if (projectilePrefab && firePoint)
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                Debug.Log($"{name} fired a projectile!");
            }
            else
            {
                Debug.LogWarning("Missing projectilePrefab or firePoint in RangedEnemy.");
            }
        }
    }
}
