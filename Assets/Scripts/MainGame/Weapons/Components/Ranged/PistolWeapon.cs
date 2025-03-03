using UnityEngine;
using Weapons.Types;

namespace Weapons.Components.Ranged
{
    public class PistolWeapon : RangedWeapon
    {
        [SerializeField]
        private float pistolDamage = 20f;

        [SerializeField]
        private float pistolAttackSpeed = 0.5f;

        [SerializeField]
        private float pistolRange = 50f;

        [SerializeField]
        private float pistolBulletSpeed = 30f;

        [SerializeField]
        private float pistolBulletLifeTime = 3f;

        private void Awake()
        {
            // Set default values for the pistol.
            weaponName = "Pistol";
            damage = pistolDamage;
            attackSpeed = pistolAttackSpeed;
            attackRange = pistolRange;

            bulletSpeed = pistolBulletSpeed;
            bulletLifeTime = pistolBulletLifeTime;

            rangeID = 0;

            // Optionally, assign or validate the attackPoint.
            if (attackPoint == null)
            {
                attackPoint = transform.Find("AttackPoint");
            }
        }

        public override void Attack()
        {
            base.Attack();
            Debug.Log($"{weaponName} fires a bullet with a special effect!");
        }
    }
}
