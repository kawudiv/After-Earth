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
            bulletCount = 1; // ✅ Always fires straight

            enemyLayers = LayerMask.GetMask("Enemy");
            environmentLayers = LayerMask.GetMask("Default");

            rangeID = 0;

            // // IK settings for the left hand
            // leftHandIKWeight = 1f;
            // leftHandIKPosition = new Vector3(-0.04f, 0.05f, 0.04f);
            // leftHandIKRotation = Quaternion.Euler(-6.9f, -167.96f, 15.6f);

            // // ✅ NEW: Set Tracking Weights for Aiming
            // bodyTrackingWeight = 0.3f;
            // gunTrackingWeight = 1f;

            if (attackPoint == null)
            {
                attackPoint = transform.Find("AttackPoint");
            }
        }
    }
}
