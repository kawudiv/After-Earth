using UnityEngine;
using Weapons.Types;

namespace Weapons.Components.Ranged
{
    public class ShotgunWeapon : RangedWeapon
    {
        protected override void Awake()
        {
            base.Awake();
            weaponName = "Shotgun";
            damage = 25f; // Lower damage per bullet
            attackSpeed = 1.2f; // Slower than other weapons
            attackRange = 30f; // Shorter range
            bulletCount = 8; // Fires 8 pellets
            spreadAngle = 10f; // Wide spread

            enemyLayers = LayerMask.GetMask("Enemy");
            environmentLayers = LayerMask.GetMask("Default");

            rangeID = 1;

            // leftHandIKWeight = 1f;
            // leftHandIKPosition = new Vector3(-0.1f, 0.06f, 0.05f);
            // leftHandIKRotation = Quaternion.Euler(-5f, -170f, 10f);

            // bodyTrackingWeight = 0.201f;
            // gunTrackingWeight = 1f;

            if (attackPoint == null)
            {
                attackPoint = transform.Find("AttackPoint");
            }
        }
    }
}
