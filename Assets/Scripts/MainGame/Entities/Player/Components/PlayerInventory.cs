using Items.Base;
using Player.Base;
using UnityEngine;
using Weapons.Base;
using Weapons.Types;
using System.Collections.Generic;  // For List

namespace Player.Components
{
    public class PlayerInventory : MonoBehaviour
    {
        private WeaponBase equippedMeleeWeapon;
        private WeaponBase equippedRangedWeapon;

        public WeaponBase EquippedMeleeWeapon => equippedMeleeWeapon;
        public WeaponBase EquippedRangedWeapon => equippedRangedWeapon;
        public WeaponBase EquippedWeapon => equippedMeleeWeapon ?? equippedRangedWeapon;

        [SerializeField] private Transform weaponHolder;
        [SerializeField] private Transform dropPoint;
        private PlayerAnimation playerAnimation;
        private PlayerBase player;
        public GameObject itemPrefab;

        // ✅ Updated: Support multiple ItemUI slots
        [SerializeField] private List<ItemUI> itemSlots = new List<ItemUI>();
        private ItemBase[] inventoryItems = new ItemBase[3];  // To track collected items

        private void Awake()
        {
            player = GetComponent<PlayerBase>();
            playerAnimation = GetComponent<PlayerAnimation>();

            if (playerAnimation == null)
            {
                Debug.LogError("[PlayerInventory] PlayerAnimation component is missing!");
            }

            // Safety check to avoid null exceptions
            if (itemSlots.Count != 3)
            {
                Debug.LogError("[PlayerInventory] Please assign exactly 3 ItemUI slots in the inspector.");
            }
        }

        public WeaponBase GetEquippedWeapon()
        {
            return equippedMeleeWeapon ?? equippedRangedWeapon;
        }

        public string GetEquippedWeaponType()
        {
            if (equippedMeleeWeapon != null)
                return "Melee";
            if (equippedRangedWeapon != null)
                return "Ranged";
            return "None";
        }

        public void TryPickUpPrefab()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
            foreach (Collider collider in colliders)
            {
                // Check for WeaponPickup
                WeaponPickup weaponPickup = collider.GetComponent<WeaponPickup>();
                if (weaponPickup != null)
                {
                    WeaponBase weaponPrefab = weaponPickup.GetWeaponPrefab();
                    if (weaponPrefab != null)
                    {
                        FindAnyObjectByType<PlayerSlotInventory>()?.AddWeapon(weaponPrefab);
                        Debug.Log($"✅ [PlayerInventory] Picked up weapon: {weaponPrefab.WeaponName}");

                        weaponPickup.gameObject.SetActive(false);
                        return;
                    }
                    else
                    {
                        Debug.LogError("[PlayerInventory] Weapon prefab is null!");
                    }
                }

                // Check for ItemPickup
                ItemPickup itemPickup = collider.GetComponent<ItemPickup>();
                if (itemPickup != null)
                {
                    ItemBase itemPrefab = itemPickup.GetItemPrefab();
                    if (itemPrefab != null)
                    {
                        if (AddItem(itemPrefab))
                        {
                            Debug.Log($"✅ [PlayerInventory] Picked up item: {itemPrefab.ItemName}");
                            itemPickup.gameObject.SetActive(false);
                        }
                        else
                        {
                            Debug.LogWarning("[PlayerInventory] Inventory full. Can't pick up item.");
                        }
                        return;
                    }
                    else
                    {
                        Debug.LogError("[PlayerInventory] Item prefab is null!");
                    }
                }
            }

            Debug.Log("[PlayerInventory] No pickup found to pick up.");
        }

        /*public bool AddItem(ItemBase newItem)
        {
            // Look for an empty slot in the inventory
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i] == null)
                {
                    inventoryItems[i] = newItem;
                    newItem.gameObject.SetActive(false);

                    // Update the UI slot if it exists
                    if (i < itemSlots.Count)
                    {
                        itemSlots[i].UpdateItemUI(newItem.ItemSprite);
                        Debug.Log($"✅ [PlayerInventory] Added item to slot {i}: {newItem.ItemName} (ID: {newItem.ItemID})");
                    }
                    else
                    {
                        Debug.LogError($"[PlayerInventory] No UI slot found for item {newItem.ItemName} at index {i}.");
                    }

                    return true;
                }
            }

            Debug.LogWarning("[PlayerInventory] No empty slot available for new item.");
            return false;
        }*/

        public bool AddItem(ItemBase newItem)
        {
            if (newItem == null)
            {
                Debug.LogError("[PlayerInventory] ❌ Cannot add a null item to the inventory!");
                return false;
            }

            // Look for an empty slot in the inventory
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i] == null)
                {
                    Debug.Log($"[PlayerInventory] 🟢 Found empty slot at index {i}.");
                    inventoryItems[i] = newItem;
                    newItem.gameObject.SetActive(false);

                    if (i < itemSlots.Count)
                    {
                        Debug.Log($"[PlayerInventory] 🔄 Updating Item UI Slot {i} with item: {newItem.ItemName}");
                        itemSlots[i].UpdateItemUI(newItem.ItemSprite);
                        Debug.Log($"✅ [PlayerInventory] Added item to slot {i}: {newItem.ItemName} (ID: {newItem.ItemID})");
                    }
                    else
                    {
                        Debug.LogError($"[PlayerInventory] ❌ No UI slot found for item {newItem.ItemName} at index {i}.");
                    }

                    return true;
                }
                else
                {
                    Debug.Log($"[PlayerInventory] Slot {i} is already occupied by: {inventoryItems[i].ItemName}");
                }
            }

            Debug.LogWarning("[PlayerInventory] ⚠️ No empty slot available for new item.");
            return false;
        }

        public void EquipWeapon(WeaponBase newWeapon)
        {
            if (newWeapon == null) return;

            UnequipWeapon();

            if (newWeapon is MeleeWeapon)
                equippedMeleeWeapon = newWeapon;
            else if (newWeapon is RangedWeapon)
                equippedRangedWeapon = newWeapon;

            newWeapon.transform.SetParent(weaponHolder);
            newWeapon.ApplyEquipTransform(newWeapon.transform);
            newWeapon.gameObject.SetActive(true);
            Debug.Log("[PlayerInventory] EquipWeapon is called");
        }

        public void DropCurrentWeapon()
        {
            if (equippedMeleeWeapon != null)
                DropWeapon(equippedMeleeWeapon);
            else if (equippedRangedWeapon != null)
                DropWeapon(equippedRangedWeapon);
            else
                Debug.Log("[PlayerInventory] No weapon equipped to drop.");
        }

        public void DropWeapon(WeaponBase weaponToDrop)
        {
            if (weaponToDrop == null) return;

            weaponToDrop.transform.SetParent(null);
            weaponToDrop.transform.position = transform.position + transform.forward * 1f;
            weaponToDrop.transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            weaponToDrop.gameObject.SetActive(true);

            Debug.Log($"✅ [PlayerInventory] Dropped {weaponToDrop.WeaponName}");

            playerAnimation?.SetTrigger("DropWeapon");
            player.PlayerInputHandler.ClearDraw();

            if (weaponToDrop is MeleeWeapon)
            {
                equippedMeleeWeapon = null;
                FindAnyObjectByType<PlayerSlotInventory>()?.ClearMeleeWeapon();
            }
            else if (weaponToDrop is RangedWeapon)
            {
                equippedRangedWeapon = null;
                FindAnyObjectByType<PlayerSlotInventory>()?.ClearRangedWeapon();
            }

            FindAnyObjectByType<PlayerSlotInventory>()?.RemoveWeaponFromInventory(weaponToDrop);
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
