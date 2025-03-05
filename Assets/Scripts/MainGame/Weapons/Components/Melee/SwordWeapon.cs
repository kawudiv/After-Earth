using UnityEngine;
using Weapons.Types;

namespace Weapons.Components.Melee
{
    public class SwordWeapon : MeleeWeapon
    {
        protected override void Awake()
        {
            base.Awake();
            weaponName = "Sword";
            damage = 10f;
            attackSpeed = 1.2f;
            attackRange = 1.5f;
            impactForce = 15f;

            enemyLayers = LayerMask.GetMask("Enemy");
            //impactEffectPrefab = Resources.Load<GameObject>("Effects/BulletImpact");


            meleeID = 0;
            // Assign or validate attackPoint
            if (attackPoint == null)
            {
                attackPoint = transform.Find("AttackPoint");
            }

            enemyLayers = LayerMask.GetMask("Enemy");
        }

        public override void Attack()
        {
            base.Attack();
            Debug.Log($"{weaponName} slashes with impact!");
        }
    }
}
