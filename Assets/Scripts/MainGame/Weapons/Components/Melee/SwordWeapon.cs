using UnityEngine;
using Weapons.Types;

namespace Weapons.Components.Melee
{
    public class SwordWeapon : MeleeWeapon
    {
        [SerializeField]
        private float swordDamage = 10f;

        [SerializeField]
        private float swordAttackSpeed = 1.2f;

        [SerializeField]
        private float swordRange = 1.5f;

        private void Awake()
        {
            weaponName = "Sword";
            damage = swordDamage;
            attackSpeed = swordAttackSpeed;
            attackRange = swordRange;

            meleeID = 0;

            if (attackPoint == null)
            {
                attackPoint = transform.Find("AttackPoint");
            }

            hitboxRadius = 1.5f;
        }

        public override void Attack()
        {
            base.Attack();
            Debug.Log($"{weaponName} slashes with a special effect!");
        }
    }
}
