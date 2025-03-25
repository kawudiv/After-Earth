using UnityEngine;
using Weapons.Types;

namespace Weapons.Components.Melee.Player
{
    public class KatanaWeapon : MeleeWeapon
    {
        protected override void Awake()
        {
            base.Awake();
            weaponName = "Katana";
            damage = 12f;  // Moderate damage
            attackSpeed = 1.5f;  // Fast attack speed
            attackRange = 1.8f;  // Slightly longer range than a sword
            impactForce = 10f;  // Less force than an axe but still strong

            entityLayers = LayerMask.GetMask("Enemy");

            meleeID = 4; // Assigning a unique melee ID

            if (attackPoint == null)
            {
                attackPoint = transform.Find("AttackPoint");
            }
        }

        public override void Attack()
        {
            base.Attack();
            Debug.Log($"{weaponName} slashes swiftly through the enemy!");
        }
    }
}
