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
            // Set default values for the sword.
            weaponName = "Sword";
            damage = swordDamage;
            attackSpeed = swordAttackSpeed;
            attackRange = swordRange;

            // Assign unique melee ID for animation blend tree.
            meleeID = 0;

            // Optionally, assign or validate the attackPoint.
            if (attackPoint == null)
            {
                attackPoint = transform.Find("AttackPoint");
            }

            // Optionally set the hitbox radius.
            hitboxRadius = 1.5f;
        }

        public override void Attack()
        {
            base.Attack();
            Debug.Log($"{weaponName} slashes with a special effect!");
        }
    }
}
