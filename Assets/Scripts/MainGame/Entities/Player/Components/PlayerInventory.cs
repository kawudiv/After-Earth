using Items.Base;
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

        public WeaponBase EquippedWeapon => equippedMeleeWeapon ?? equippedRangedWeapon;

        [SerializeField]
        private Transform weaponHolder;

        [SerializeField]
        private Transform dropPoint;
        private PlayerAnimation playerAnimation;
        private PlayerBase player;
        public GameObject itemPrefab;

        [SerializeField]
        private ItemUI itemUI;

        private void Awake()
        {
            player = GetComponent<PlayerBase>();
            playerAnimation = GetComponent<PlayerAnimation>();

            if (playerAnimation == null)
            {
                Debug.LogError("[PlayerInventory] PlayerAnimation component is missing!");
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
                        // Directly add the existing weapon to the inventory without instantiating
                        FindAnyObjectByType<PlayerSlotInventory>()
                            ?.AddWeapon(weaponPrefab);
                        Debug.Log(
                            $"✅ [PlayerInventory] Picked up weapon: {weaponPrefab.WeaponName}"
                        );

                        // Disable the pickup object in the scene
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
                        // Directly add the existing item to the inventory without instantiating
                        FindAnyObjectByType<PlayerSlotInventory>()
                            ?.AddItem(itemPrefab);
                        Debug.Log($"✅ [PlayerInventory] Picked up item: {itemPrefab.ItemName}");

                        // Update the ItemCollectedSlot image with the item's sprite
                        if (itemUI != null)
                        {
                            itemUI.UpdateItemUI(itemPrefab.ItemSprite);
                        }

                        itemPickup.gameObject.SetActive(false);
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

        public void EquipWeapon(WeaponBase newWeapon)
        {
            if (newWeapon == null)
                return;

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
            if (weaponToDrop == null)
                return;

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

        public void AddItem(ItemBase newItem)
        {
            newItem.gameObject.SetActive(false); // Disable the item in the scene
            Debug.Log($"✅ [PlayerInventory] Added item: {newItem.ItemName} (ID: {newItem.ItemID})");
        }
    }
}