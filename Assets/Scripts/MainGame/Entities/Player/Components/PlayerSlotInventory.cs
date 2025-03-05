using System.Collections.Generic;
using Items.Base;
using UnityEngine;
using Weapons.Base;
using Weapons.Types;

namespace Player.Components
{
    public class PlayerSlotInventory : MonoBehaviour
    {
        [SerializeField]
        private PlayerInventory playerInventory;

        [SerializeField]
        private WeaponUI weaponUI;

        [SerializeField] // Make the list visible in the Inspector
        private List<ItemBase> items = new List<ItemBase>(); // List to store all items

        private WeaponBase meleeWeaponSlot;
        private WeaponBase rangedWeaponSlot;

        private void Start()
        {
            if (playerInventory == null)
            {
                Debug.LogError("[PlayerSlotInventory] PlayerInventory is not assigned!");
            }

            if (weaponUI == null)
            {
                Debug.LogError("[PlayerSlotInventory] WeaponUI is not assigned!");
            }

            // Initial UI update (no weapon equipped)
            weaponUI.UpdateWeaponUI(false, false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ToggleMeleeWeapon();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ToggleRangedWeapon();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                DropEquippedWeapon();
            }
        }

        private void DropEquippedWeapon()
        {
            if (playerInventory.EquippedMeleeWeapon != null)
            {
                playerInventory.DropWeapon(playerInventory.EquippedMeleeWeapon);
                weaponUI.UpdateWeaponUI(false, rangedWeaponSlot != null);
            }
            else if (playerInventory.EquippedRangedWeapon != null)
            {
                playerInventory.DropWeapon(playerInventory.EquippedRangedWeapon);
                weaponUI.UpdateWeaponUI(meleeWeaponSlot != null, false);
            }
        }

        public void RemoveWeaponFromInventory(WeaponBase weapon)
        {
            if (weapon == meleeWeaponSlot)
            {
                meleeWeaponSlot = null;
            }
            else if (weapon == rangedWeaponSlot)
            {
                rangedWeaponSlot = null;
            }
        }

        public void ClearMeleeWeapon()
        {
            meleeWeaponSlot = null;
        }

        public void ClearRangedWeapon()
        {
            rangedWeaponSlot = null;
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

            newWeapon.gameObject.SetActive(false);
        }

        private void ToggleMeleeWeapon()
        {
            if (playerInventory.EquippedMeleeWeapon == meleeWeaponSlot)
            {
                playerInventory.UnequipWeapon();
                weaponUI.UpdateWeaponUI(false, playerInventory.EquippedRangedWeapon != null);
            }
            else if (meleeWeaponSlot != null)
            {
                playerInventory.UnequipWeapon();
                playerInventory.EquipWeapon(meleeWeaponSlot);
                weaponUI.UpdateWeaponUI(true, false);
            }
        }

        private void ToggleRangedWeapon()
        {
            if (playerInventory.EquippedRangedWeapon == rangedWeaponSlot)
            {
                playerInventory.UnequipWeapon();
                weaponUI.UpdateWeaponUI(playerInventory.EquippedMeleeWeapon != null, false);
            }
            else if (rangedWeaponSlot != null)
            {
                playerInventory.UnequipWeapon();
                playerInventory.EquipWeapon(rangedWeaponSlot);
                weaponUI.UpdateWeaponUI(false, true);
            }
        }

        public void AddItem(ItemBase newItem)
        {
            if (newItem == null)
            {
                Debug.LogError("[PlayerSlotInventory] Cannot add a null item to the inventory!");
                return;
            }

            // Add the item to the inventory
            items.Add(newItem);
            Debug.Log(
                $"✅ [PlayerSlotInventory] Added item: {newItem.ItemName} (ID: {newItem.ItemID})"
            );
        }
    }
}
