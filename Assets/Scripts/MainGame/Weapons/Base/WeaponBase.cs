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

        [Header("Equipped Transform Offsets")]
        [SerializeField]
        private Vector3 equipPositionOffset;

        [SerializeField]
        private Vector3 equipRotationOffset;

        // Changed to protected to allow derived classes to access
        protected HashSet<IDamageable> damagedEnemies = new HashSet<IDamageable>();

        public string WeaponName => weaponName;
        public int WeaponTypeID => weaponTypeID;
        public float Damage => damage;
        public float AttackSpeed => attackSpeed;
        public float AttackRange => attackRange;

        protected virtual void Awake()
        {
            // Base initialization
        }

        public void ApplyEquipTransform(Transform weaponTransform)
        {
            weaponTransform.localPosition = equipPositionOffset;
            weaponTransform.localRotation = Quaternion.Euler(equipRotationOffset);
        }

        public abstract void Attack();

        // Made virtual so derived classes can extend if needed
        protected virtual void ResetHitRecords()
        {
            if (damagedEnemies != null)
            {
                damagedEnemies.Clear();
            }
            else
            {
                damagedEnemies = new HashSet<IDamageable>();
            }
            Debug.Log($"[WeaponBase] Cleared damage records for {weaponName}");
        }

        // New helper method to check if enemy was already damaged
        protected bool HasDamaged(IDamageable damageable)
        {
            if (damagedEnemies == null)
            {
                damagedEnemies = new HashSet<IDamageable>();
                return false;
            }
            return damagedEnemies.Contains(damageable);
        }

        // New helper method to mark enemy as damaged
        protected void MarkAsDamaged(IDamageable damageable)
        {
            if (damagedEnemies == null)
            {
                damagedEnemies = new HashSet<IDamageable>();
            }
            damagedEnemies.Add(damageable);
        }

        // Cleanup when weapon is destroyed
        protected virtual void OnDestroy()
        {
            if (damagedEnemies != null)
            {
                damagedEnemies.Clear();
            }
        }
    }
}
