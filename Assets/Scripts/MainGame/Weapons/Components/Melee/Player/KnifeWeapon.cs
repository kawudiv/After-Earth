using UnityEngine;
using Weapons.Types;

namespace Weapons.Components.Melee.Player
{
    public class KnifeWeapon : MeleeWeapon
    {
        protected override void Awake()
        {
            base.Awake();
            weaponName = "Knife";
            damage = 5f;
            attackSpeed = 2.5f;
            attackRange = 0.8f;
            impactForce = 5f;

            entityLayers = LayerMask.GetMask("Enemy");

            meleeID = 2;

            if (attackPoint == null)
            {
                attackPoint = transform.Find("AttackPoint");
            }
        }

        public override void Attack()
        {
            base.Attack();
            Debug.Log($"{weaponName} strikes swiftly!");
        }
    }
}
