using UnityEngine;
using Weapons.Base;
using Weapons.Types;

namespace Player.Components
{
    public class PlayerSlotInventory : MonoBehaviour
    {
        [SerializeField] private PlayerInventory playerInventory;

        private WeaponBase meleeWeaponSlot;
        private WeaponBase rangedWeaponSlot;

        private void Start()
        {
            if (playerInventory == null)
            {
                Debug.LogError("[PlayerSlotInventory] PlayerInventory is not assigned!");
            }
        }

        private void Update()
        {
            // Switch or unequip weapons using 1 & 2 keys
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ToggleMeleeWeapon();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ToggleRangedWeapon();
            }
        }

        public void AddWeapon(WeaponBase newWeapon)
        {
            if (newWeapon is MeleeWeapon)
            {
                meleeWeaponSlot = newWeapon;
            }
            else if (newWeapon is RangedWeapon)
            {
                rangedWeaponSlot = newWeapon;
            }

            newWeapon.gameObject.SetActive(false); // Hide weapon when added to inventory
            Debug.Log($"✅ [PlayerSlotInventory] Stored {newWeapon.weaponName} in {(newWeapon is MeleeWeapon ? "Melee" : "Ranged")} Slot.");
        }


        private void ToggleMeleeWeapon()
        {
            if (playerInventory.EquippedMeleeWeapon == meleeWeaponSlot)
            {
                playerInventory.UnequipWeapon();
            }
            else if (meleeWeaponSlot != null)
            {
                playerInventory.UnequipWeapon(); // Unequip current weapon before equipping
                playerInventory.EquipWeapon(meleeWeaponSlot);
            }
        }

        private void ToggleRangedWeapon()
        {
            if (playerInventory.EquippedRangedWeapon == rangedWeaponSlot)
            {
                playerInventory.UnequipWeapon();
            }
            else if (rangedWeaponSlot != null)
            {
                playerInventory.UnequipWeapon();
                playerInventory.EquipWeapon(rangedWeaponSlot);
            }
        }

    }
}
