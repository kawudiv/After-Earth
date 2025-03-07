using UnityEngine;
using Weapons.Types;

namespace Weapons.Components.Ranged
{
    public class PistolWeapon : RangedWeapon
    {
        protected override void Awake()
        {
            base.Awake();
            weaponName = "Pistol";
            damage = 20f;
            attackSpeed = 0.5f;
            attackRange = 50f;
            bulletSpeed = 30f;
            bulletLifeTime = 3f;
            bulletForce = 10f;

            // Assign layer masks & impact effects
            enemyLayers = LayerMask.GetMask("Enemy");
            environmentLayers = LayerMask.GetMask("Default");
            //impactEffectPrefab = Resources.Load<GameObject>("Effects/BulletImpact");


            rangeID = 0;

            // Assign or validate attackPoint
            if (attackPoint == null)
            {
                attackPoint = transform.Find("AttackPoint");
            }
        }

        public override void Attack()
        {
            base.Attack();
            Debug.Log($"{weaponName} fires a bullet!");
        }
    }
}
