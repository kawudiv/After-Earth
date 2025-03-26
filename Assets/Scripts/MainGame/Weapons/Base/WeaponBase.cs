using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Weapons.Base
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [Header("Base Weapon Stats")]
        [SerializeField]
        protected string weaponName;

        [SerializeField]
        public int weaponTypeID;

        [SerializeField]
        protected float damage;

        [SerializeField]
        public float attackSpeed;

        [SerializeField]
        protected float attackRange;

        // ✅ Serialized Transform Offsets (for equipping)
        [Header("Equipped Transform Offsets")]
        [SerializeField]
        private Vector3 equipPositionOffset;

        [SerializeField]
        private Vector3 equipRotationOffset;

        protected HashSet<IDamageable> damagedEnemies = new HashSet<IDamageable>();

        public string WeaponName => weaponName;
        public int WeaponTypeID => weaponTypeID;
        public float Damage => damage;
        public float AttackSpeed => attackSpeed;
        public float AttackRange => attackRange;

        protected virtual void Awake()
        {
            // Base setup, no collider here!
        }

        // ✅ Automatically apply equip offsets
        public void ApplyEquipTransform(Transform weaponTransform)
        {
            weaponTransform.localPosition = equipPositionOffset;
            weaponTransform.localRotation = Quaternion.Euler(equipRotationOffset);
        }

        // Each weapon must implement its own attack logic.
        public abstract void Attack();

        protected void ResetHitRecords()
        {
            damagedEnemies.Clear();
        }
    }
}
