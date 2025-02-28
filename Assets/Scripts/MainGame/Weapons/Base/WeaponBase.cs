using UnityEngine;

namespace Weapons.Base
{
    public abstract class WeaponBase : MonoBehaviour
    {
        // Unique ID for animation blending
        public int weaponTypeID;

        // Basic properties of the weapon.
        public string weaponName;
        public float damage; // Additional damage provided by the weapon.
        public float attackSpeed; // Time between attacks.
        public float attackRange; // Effective range (useful for both melee and ranged).

        // ✅ Position and Rotation Offsets for Equipping
        [Header("Equipped Transform Offsets")]
        [SerializeField] private Vector3 equipPositionOffset;
        [SerializeField] private Vector3 equipRotationOffset;

        // Optionally, you can add an initialization method to set up the weapon.
        public virtual void Initialize()
        {
            // Initialization code if needed.
        }

        // ✅ Apply offsets when equipping
        public void ApplyEquipTransform(Transform weaponTransform)
        {
            weaponTransform.localPosition = equipPositionOffset;
            weaponTransform.localRotation = Quaternion.Euler(equipRotationOffset);
        }

        // Each weapon must implement its own attack logic.
        public abstract void Attack();
    }
}
