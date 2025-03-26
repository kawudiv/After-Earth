using UnityEngine;
using Weapons.Types;

namespace Weapons.Components.Melee.Player
{
    public class AxeWeapon : MeleeWeapon
    {
        protected override void Awake()
        {
            base.Awake();
            weaponName = "Axe";
            damage = 60f;
            attackSpeed = 1.0f;
            attackRange = 1.7f;
            impactForce = 20f;

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
            Debug.Log($"{weaponName} chops down with force!");
        }
    }
}
