using UnityEngine;
using Weapons.Base;
using Weapons.Types;

namespace Player.Components
{
    public class PlayerInventory : MonoBehaviour
    {
        private WeaponBase equippedMeleeWeapon;
        private WeaponBase equippedRangedWeapon;

        [SerializeField]
        private Transform weaponHolder; // ✅ Assign this in the Inspector (RightHandWeaponSlot)

        [SerializeField]
        private Transform dropPoint; // ✅ Assign this in the Inspector

        private PlayerAnimation playerAnimation; // ✅ Reference to animation controller

        private void Awake()
        {
            playerAnimation = GetComponent<PlayerAnimation>();

            if (playerAnimation == null)
            {
                Debug.LogError("[PlayerInventory] PlayerAnimation component is missing!");
            }
        }

        public void TryPickUpWeapon()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
            foreach (Collider collider in colliders)
            {
                ItemPickup pickup = collider.GetComponent<ItemPickup>();
                if (pickup != null)
                {
                    pickup.PickupWeapon(this);
                    return;
                }
            }
            Debug.Log("[PlayerInventory] No weapon found to pick up.");
        }

        public void EquipWeapon(WeaponBase newWeapon)
        {
            if (newWeapon == null)
            {
                Debug.LogError("[PlayerInventory] Trying to equip a NULL weapon!");
                return;
            }

            Debug.Log(
                $"[PlayerInventory] Attempting to equip {newWeapon.weaponName} ({newWeapon.GetType().Name})"
            );

            if (newWeapon is MeleeWeapon melee)
            {
                if (equippedMeleeWeapon != null)
                {
                    Debug.Log(
                        "[PlayerInventory] Dropping old melee weapon before equipping new one."
                    );
                    DropWeapon(equippedMeleeWeapon);
                }

                equippedMeleeWeapon = melee;
                equippedMeleeWeapon.transform.SetParent(weaponHolder);

                // ✅ Apply Position & Rotation Offsets
                equippedMeleeWeapon.ApplyEquipTransform(equippedMeleeWeapon.transform);

                // ✅ Set the correct animation type for the melee weapon
                if (playerAnimation != null)
                {
                    playerAnimation.SetMeleeWeaponType(melee.weaponTypeID);
                    playerAnimation.SetTrigger("Draw"); // ✅ Play Draw Animation
                }

                Debug.Log(
                    $"✅ [PlayerInventory] Melee Weapon Equipped: {equippedMeleeWeapon.weaponName}"
                );
            }
            else if (newWeapon is RangedWeapon ranged)
            {
                if (equippedRangedWeapon != null)
                {
                    Debug.Log(
                        "[PlayerInventory] Dropping old ranged weapon before equipping new one."
                    );
                    DropWeapon(equippedRangedWeapon);
                }

                equippedRangedWeapon = ranged;
                equippedRangedWeapon.transform.SetParent(weaponHolder);

                // ✅ Apply Position & Rotation Offsets
                equippedRangedWeapon.ApplyEquipTransform(equippedRangedWeapon.transform);

                Debug.Log(
                    $"✅ [PlayerInventory] Ranged Weapon Equipped: {equippedRangedWeapon.weaponName}"
                );
            }
        }

        public void DropCurrentWeapon()
        {
            if (equippedMeleeWeapon == null && equippedRangedWeapon == null)
            {
                Debug.Log("[PlayerInventory] No weapon equipped to drop.");
                return;
            }

            if (equippedMeleeWeapon != null)
            {
                DropWeapon(equippedMeleeWeapon);
                equippedMeleeWeapon = null;

                // ✅ Reset animation state if weapon is dropped
                if (playerAnimation != null)
                {
                    playerAnimation.SetMeleeWeaponType(-1); // -1 means no weapon equipped
                }
            }
            else if (equippedRangedWeapon != null)
            {
                DropWeapon(equippedRangedWeapon);
                equippedRangedWeapon = null;
            }
        }

        public void DropWeapon(WeaponBase weaponToDrop)
        {
            if (weaponToDrop == null)
                return;

            // ✅ Remove it from the player's hand
            weaponToDrop.transform.SetParent(null);

            // ✅ Drop in front of the player, ignoring DropPoint if it exists
            weaponToDrop.transform.position = transform.position + transform.forward * 1f;

            weaponToDrop.transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

            weaponToDrop.gameObject.SetActive(true);

            Debug.Log($"✅ [PlayerInventory] Dropped {weaponToDrop.weaponName}");

            if (weaponToDrop is MeleeWeapon)
            {
                equippedMeleeWeapon = null;
            }
            else
            {
                equippedRangedWeapon = null;
            }
        }
    }
}
