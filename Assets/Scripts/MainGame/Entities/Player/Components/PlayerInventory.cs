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
        private Transform weaponHolder; // ✅ Assign in Inspector (RightHandWeaponSlot)

        [SerializeField]
        private Transform dropPoint; // ✅ Assign in Inspector

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

            Debug.Log($"[PlayerInventory] Attempting to equip {newWeapon.weaponName}");

            if (newWeapon is MeleeWeapon melee)
            {
                // ✅ Destroy the old weapon instead of dropping it
                if (equippedMeleeWeapon != null)
                {
                    Debug.Log(
<<<<<<< Updated upstream
                        "[PlayerInventory] Disabling old melee weapon before equipping new one."
                    );
                    equippedMeleeWeapon.gameObject.SetActive(false); // ✅ Disable instead of destroy
=======
                        "[PlayerInventory] Destroying old melee weapon before equipping new one."
                    );
                    Destroy(equippedMeleeWeapon.gameObject);
>>>>>>> Stashed changes
                }

                equippedMeleeWeapon = melee;
                equippedMeleeWeapon.transform.SetParent(weaponHolder);
                equippedMeleeWeapon.ApplyEquipTransform(equippedMeleeWeapon.transform);

                if (playerAnimation != null)
                {
                    playerAnimation.SetMeleeWeaponType(melee.weaponTypeID);
                    playerAnimation.SetTrigger("DrawMelee");
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
<<<<<<< Updated upstream
                if (playerAnimation != null)
                {
                    playerAnimation.SetTrigger("Sheath"); // ✅ Fix: Return to default blend tree
                    playerAnimation.SetMeleeWeaponType(-1); // ✅ Reset weapon type
=======
                // ✅ Automatically Sheath before dropping
                if (playerAnimation != null)
                {
                    playerAnimation.SetTrigger("Sheath"); // ✅ Fix: Go back to default blend tree
                    playerAnimation.SetMeleeWeaponType(-1); // ✅ Reset melee weapon type
>>>>>>> Stashed changes
                }

                DropWeapon(equippedMeleeWeapon);
                equippedMeleeWeapon = null;
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

<<<<<<< Updated upstream
            // ✅ Unparent and move weapon to drop location
            weaponToDrop.transform.SetParent(null);
            weaponToDrop.transform.position = dropPoint.position; // ✅ Drop at predefined position
            weaponToDrop.transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

            // ✅ Reactivate dropped weapon
=======
            // ✅ Unparent the weapon
            weaponToDrop.transform.SetParent(null);

            // ✅ Move the weapon in front of the player before unassigning it
            weaponToDrop.transform.position = transform.position + transform.forward * 1f;
            weaponToDrop.transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

            // ✅ Ensure the dropped weapon is active
>>>>>>> Stashed changes
            weaponToDrop.gameObject.SetActive(true);
            Debug.Log($"✅ [PlayerInventory] Dropped {weaponToDrop.weaponName}");

<<<<<<< Updated upstream
            // ✅ Unassign reference to prevent memory leaks
=======
            // ✅ Unassign the weapon *before* modifying the reference
>>>>>>> Stashed changes
            if (weaponToDrop is MeleeWeapon)
            {
                equippedMeleeWeapon = null;
            }
            else if (weaponToDrop is RangedWeapon)
            {
                equippedRangedWeapon = null;
            }

<<<<<<< Updated upstream
            weaponToDrop = null; // ✅ Clear reference
=======
            weaponToDrop = null; // ✅ Ensure no lingering reference remains
>>>>>>> Stashed changes
        }

        public bool HasMeleeWeapon()
        {
            return equippedMeleeWeapon != null;
        }
    }
}
