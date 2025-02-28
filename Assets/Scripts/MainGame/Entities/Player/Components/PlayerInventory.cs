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
                equippedMeleeWeapon.transform.SetParent(weaponHolder); // Attach to player

                // ✅ Apply Position & Rotation Offsets
                equippedMeleeWeapon.ApplyEquipTransform(equippedMeleeWeapon.transform);

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
                equippedRangedWeapon.transform.SetParent(weaponHolder); // Attach to player

                // ✅ Apply Position & Rotation Offsets
                equippedRangedWeapon.ApplyEquipTransform(equippedRangedWeapon.transform);

                Debug.Log(
                    $"✅ [PlayerInventory] Ranged Weapon Equipped: {equippedRangedWeapon.weaponName}"
                );
            }
        }

        private void AttachWeaponToHand(WeaponBase weapon)
        {
            if (weaponHolder == null)
            {
                Debug.LogError(
                    "[PlayerInventory] Weapon Holder is not assigned! Assign RightHandWeaponSlot."
                );
                return;
            }

            // Attach the weapon to the player's hand
            weapon.transform.SetParent(weaponHolder);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;

            // ✅ Ensure the weapon is visible and activated
            weapon.gameObject.SetActive(true);

            // ✅ Ensure the MeshRenderer is enabled
            MeshRenderer meshRenderer = weapon.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }
            else
            {
                Debug.LogWarning(
                    $"[PlayerInventory] {weapon.weaponName} does not have a MeshRenderer component!"
                );
            }

            Debug.Log($"✅ [PlayerInventory] {weapon.weaponName} attached to {weaponHolder.name}");
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
