using UnityEngine;
using Weapons.Types;

namespace Weapons.Components.Melee.Enemy
{
    public class ShovelWeapon : MeleeWeapon
    {
        protected override void Awake()
        {
            base.Awake();
            weaponName = "Enemy Shovel";
            damage = 10f;
            attackSpeed = 1.2f;
            attackRange = 1.5f;
            impactForce = 15f;
            meleeID = 100;

            // Only target player layer
            entityLayers = LayerMask.GetMask("Player");

            // Assign attack point
            if (attackPoint == null)
            {
                attackPoint = transform.Find("AttackPoint");
            }
        }

        public override void Attack()
        {
            base.Attack();
            Debug.Log($"[{weaponName}] Enemy performing shovel attack!");
        }
    }
}
