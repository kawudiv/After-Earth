using System.Collections.Generic;
using Items.Base;
using Player.Base;
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

        [SerializeField]
        private List<ItemBase> items = new List<ItemBase>();

        private WeaponBase meleeWeaponSlot;
        private WeaponBase rangedWeaponSlot;
        private PlayerBase playerBase;

        private void Start()
        {
            playerBase = GetComponent<PlayerBase>();
            if (playerInventory == null)
            {
                Debug.LogError("[PlayerSlotInventory] ❌ PlayerInventory is not assigned!");
            }

            if (weaponUI == null)
            {
                Debug.LogError("[PlayerSlotInventory] ❌ WeaponUI is not assigned!");
            }

            Debug.Log("[PlayerSlotInventory] ✅ Inventory initialized.");

            // Initial UI update (no weapon equipped)
            weaponUI.UpdateWeaponUI(false, false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("[PlayerSlotInventory] 🔄 Toggle Melee Weapon.");
                ToggleMeleeWeapon();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("[PlayerSlotInventory] 🔄 Toggle Ranged Weapon.");
                ToggleRangedWeapon();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("[PlayerSlotInventory] 🗑 Dropping equipped weapon.");
                DropEquippedWeapon();
            }
        }

        private void DropEquippedWeapon()
        {
            if (playerInventory.EquippedMeleeWeapon != null)
            {
                Debug.Log(
                    $"[PlayerSlotInventory] 🗑 Dropping melee weapon: {playerInventory.EquippedMeleeWeapon.WeaponName}"
                );
                playerInventory.DropWeapon(playerInventory.EquippedMeleeWeapon);
                weaponUI.UpdateWeaponUI(false, rangedWeaponSlot != null);
            }
            else if (playerInventory.EquippedRangedWeapon != null)
            {
                Debug.Log(
                    $"[PlayerSlotInventory] 🗑 Dropping ranged weapon: {playerInventory.EquippedRangedWeapon.WeaponName}"
                );
                playerInventory.DropWeapon(playerInventory.EquippedRangedWeapon);
                weaponUI.UpdateWeaponUI(meleeWeaponSlot != null, false);
            }
            else
            {
                Debug.Log("[PlayerSlotInventory] ❌ No weapon equipped to drop.");
            }
        }

        public void RemoveWeaponFromInventory(WeaponBase weapon)
        {
            if (weapon == meleeWeaponSlot)
            {
                Debug.Log($"[PlayerSlotInventory] ❌ Removed melee weapon: {weapon.WeaponName}");
                meleeWeaponSlot = null;
            }
            else if (weapon == rangedWeaponSlot)
            {
                Debug.Log($"[PlayerSlotInventory] ❌ Removed ranged weapon: {weapon.WeaponName}");
                rangedWeaponSlot = null;
            }
        }

        public void ClearMeleeWeapon()
        {
            Debug.Log("[PlayerSlotInventory] 🗑 Clearing melee weapon slot.");
            meleeWeaponSlot = null;
        }

        public void ClearRangedWeapon()
        {
            Debug.Log("[PlayerSlotInventory] 🗑 Clearing ranged weapon slot.");
            rangedWeaponSlot = null;
        }

        public void AddWeapon(WeaponBase newWeapon)
        {
            if (newWeapon is MeleeWeapon)
            {
                meleeWeaponSlot = newWeapon;
                Debug.Log($"[PlayerSlotInventory] ✅ Added melee weapon: {newWeapon.WeaponName}");
            }
            else if (newWeapon is RangedWeapon)
            {
                rangedWeaponSlot = newWeapon;
                Debug.Log($"[PlayerSlotInventory] ✅ Added ranged weapon: {newWeapon.WeaponName}");
            }

            newWeapon.gameObject.SetActive(false);
        }

        private void ToggleMeleeWeapon()
        {
            if (playerInventory.EquippedMeleeWeapon is MeleeWeapon meleeWeapon)
            {
                Debug.Log(
                    $"[PlayerSlotInventory] 🔄 Unequipping melee weapon: {meleeWeapon.WeaponName}"
                );
                meleeWeapon.OnDrop();
                playerBase.PlayerAnimation.SetDrawSheathAnimation(false);
                playerInventory.UnequipWeapon();
                weaponUI.UpdateWeaponUI(false, playerInventory.EquippedRangedWeapon != null);
            }
            else if (meleeWeaponSlot is MeleeWeapon newMeleeWeapon)
            {
                Debug.Log(
                    $"[PlayerSlotInventory] 🔄 Equipping melee weapon: {newMeleeWeapon.WeaponName}"
                );
                newMeleeWeapon.OnPickup();
                playerBase.PlayerAnimation.SetMeleeWeaponType(newMeleeWeapon.meleeID);
                playerBase.PlayerAnimation.SetDrawSheathAnimation(true);
                playerInventory.UnequipWeapon();
                playerInventory.EquipWeapon(newMeleeWeapon);
                weaponUI.UpdateWeaponUI(true, false);
            }
            else
            {
                Debug.Log("[PlayerSlotInventory] ❌ No melee weapon to equip.");
            }
        }

        private void ToggleRangedWeapon()
        {
            if (playerInventory.EquippedRangedWeapon == rangedWeaponSlot)
            {
                Debug.Log(
                    $"[PlayerSlotInventory] 🔄 Unequipping ranged weapon: {rangedWeaponSlot.WeaponName}"
                );
                playerInventory.UnequipWeapon();
                weaponUI.UpdateWeaponUI(playerInventory.EquippedMeleeWeapon != null, false);
            }
            else if (rangedWeaponSlot != null)
            {
                Debug.Log(
                    $"[PlayerSlotInventory] 🔄 Equipping ranged weapon: {rangedWeaponSlot.WeaponName}"
                );
                playerInventory.UnequipWeapon();
                playerInventory.EquipWeapon(rangedWeaponSlot);
                weaponUI.UpdateWeaponUI(false, true);
            }
            else
            {
                Debug.Log("[PlayerSlotInventory] ❌ No ranged weapon to equip.");
            }
        }

        public void AddItem(ItemBase newItem)
        {
            if (newItem == null)
            {
                Debug.LogError("[PlayerSlotInventory] ❌ Cannot add a null item to the inventory!");
                return;
            }

            items.Add(newItem);
            Debug.Log(
                $"✅ [PlayerSlotInventory] Added item: {newItem.ItemName} (ID: {newItem.ItemID})"
            );
        }
    }
}
