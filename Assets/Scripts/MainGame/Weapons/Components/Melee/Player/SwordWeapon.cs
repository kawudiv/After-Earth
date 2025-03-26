using UnityEngine;
using Weapons.Types;

namespace Weapons.Components.Melee.Player
{
    public class SwordWeapon : MeleeWeapon
    {
        protected override void Awake()
        {
            base.Awake();
            weaponName = "Sword";
            damage = 35f;
            attackSpeed = 1.2f;
            attackRange = 1.5f;
            impactForce = 15f;

            entityLayers = LayerMask.GetMask("Enemy");
            //impactEffectPrefab = Resources.Load<GameObject>("Effects/BulletImpact");


            meleeID = 0;
            // Assign or validate attackPoint
            if (attackPoint == null)
            {
                attackPoint = transform.Find("AttackPoint");
            }
        }

        public override void Attack()
        {
            base.Attack();
            Debug.Log($"{weaponName} slashes with impact!");
        }
    }
}
