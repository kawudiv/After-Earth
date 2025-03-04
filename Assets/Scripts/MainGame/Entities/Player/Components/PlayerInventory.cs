using Player.Base;
using UnityEngine;
using Weapons.Base;
using Weapons.Types;

namespace Player.Components
{
    public class PlayerInventory : MonoBehaviour
    {
        private WeaponBase equippedMeleeWeapon;
        private WeaponBase equippedRangedWeapon;

        public WeaponBase EquippedMeleeWeapon => equippedMeleeWeapon;
        public WeaponBase EquippedRangedWeapon => equippedRangedWeapon;

        [SerializeField]
        private Transform weaponHolder;

        [SerializeField]
        private Transform dropPoint;
        private PlayerAnimation playerAnimation;
        private PlayerBase player;

        private void Awake()
        {
            player = GetComponent<PlayerBase>();
            playerAnimation = GetComponent<PlayerAnimation>();

            if (playerAnimation == null)
            {
                Debug.LogError("[PlayerInventory] PlayerAnimation component is missing!");
            }
        }

        /// <summary>
        /// Returns the currently equipped weapon (melee or ranged).
        /// </summary>
        /// <returns>The currently equipped weapon, or null if no weapon is equipped.</returns>
        public WeaponBase GetEquippedWeapon()
        {
            if (equippedMeleeWeapon != null)
            {
                return equippedMeleeWeapon;
            }
            else if (equippedRangedWeapon != null)
            {
                return equippedRangedWeapon;
            }
            return null;
        }

        /// <summary>
        /// Returns the type of the currently equipped weapon.
        /// </summary>
        /// <returns>"Melee", "Ranged", or "None" if no weapon is equipped.</returns>
        public string GetEquippedWeaponType()
        {
            if (equippedMeleeWeapon != null)
            {
                return "Melee";
            }
            else if (equippedRangedWeapon != null)
            {
                return "Ranged";
            }
            return "None";
        }

        public void TryPickUpWeapon()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
            foreach (Collider collider in colliders)
            {
                ItemPickup pickup = collider.GetComponent<ItemPickup>();
                if (pickup != null)
                {
                    WeaponBase weapon = pickup.weaponPrefab;

                    FindAnyObjectByType<PlayerSlotInventory>()?.AddWeapon(weapon);

                    // Hide the pickup item
                    pickup.gameObject.SetActive(false);
                    return;
                }
            }
            Debug.Log("[PlayerInventory] No weapon found to pick up.");
        }

        public void EquipWeapon(WeaponBase newWeapon)
        {
            if (newWeapon == null) return;

            if (newWeapon is MeleeWeapon melee)
            {
                if (equippedMeleeWeapon != null)
                {
                    UnequipWeapon();
                }
                equippedMeleeWeapon = melee;
            }
            else if (newWeapon is RangedWeapon ranged)
            {
                if (equippedRangedWeapon != null)
                {
                    UnequipWeapon();
                }
                equippedRangedWeapon = ranged;
            }

            newWeapon.transform.SetParent(weaponHolder);
            newWeapon.ApplyEquipTransform(newWeapon.transform);
            newWeapon.gameObject.SetActive(true); 
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

            // ✅ Drop in front of the player
            weaponToDrop.transform.position = transform.position + transform.forward * 1f;
            weaponToDrop.transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            weaponToDrop.gameObject.SetActive(true);

            Debug.Log($"✅ [PlayerInventory] Dropped {weaponToDrop.weaponName}");

            if (playerAnimation != null)
            {
                playerAnimation.SetTrigger("DropWeapon");
                WeaponDrawnToggle(false);
            }

            if (weaponToDrop is MeleeWeapon)
            {
                equippedMeleeWeapon = null;
            }
            else
            {
                equippedRangedWeapon = null;
            }
        }

        public void WeaponDrawnToggle(bool value)
        {
            player.IsWeaponDrawn = value;
        }

        public void UnequipWeapon()
        {
            if (equippedMeleeWeapon != null)
            {
                equippedMeleeWeapon.gameObject.SetActive(false);
                equippedMeleeWeapon = null;
                Debug.Log("❌ [PlayerInventory] Melee Weapon Unequipped");
            }
            else if (equippedRangedWeapon != null)
            {
                equippedRangedWeapon.gameObject.SetActive(false);
                equippedRangedWeapon = null;
                Debug.Log("❌ [PlayerInventory] Ranged Weapon Unequipped");
            }
        }

    }
}
