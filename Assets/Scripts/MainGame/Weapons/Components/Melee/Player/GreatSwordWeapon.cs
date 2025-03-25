using UnityEngine;
using Weapons.Types;

namespace Weapons.Components.Melee.Player
{
    public class GreatSwordWeapon : MeleeWeapon
    {
        protected override void Awake()
        {
            base.Awake();
            weaponName = "Great Sword";
            damage = 25f;
            attackSpeed = 0.8f;
            attackRange = 2.0f;
            impactForce = 30f;

            entityLayers = LayerMask.GetMask("Enemy");

            meleeID = 1;

            if (attackPoint == null)
            {
                attackPoint = transform.Find("AttackPoint");
            }
        }

        public override void Attack()
        {
            base.Attack();
            Debug.Log($"{weaponName} swings with massive force!");
        }
    }
}
