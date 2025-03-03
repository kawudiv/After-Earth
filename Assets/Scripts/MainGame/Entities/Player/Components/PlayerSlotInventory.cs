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
            // Switch weapons using 1 & 2 keys
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchToMeleeWeapon();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchToRangedWeapon();
            }
        }

        //Adds a weapon to the appropriate slot
        public void AddWeapon(WeaponBase newWeapon)
        {
            if (newWeapon is MeleeWeapon)
            {
                if (meleeWeaponSlot != null)
                {
                    Debug.Log("[PlayerSlotInventory] Replacing existing melee weapon.");
                    playerInventory.DropWeapon(meleeWeaponSlot);
                }
                meleeWeaponSlot = newWeapon;
                Debug.Log($"✅ [PlayerSlotInventory] Stored {newWeapon.weaponName} in Melee Slot.");
            }
            else if (newWeapon is RangedWeapon)
            {
                if (rangedWeaponSlot != null)
                {
                    Debug.Log("[PlayerSlotInventory] Replacing existing ranged weapon.");
                    playerInventory.DropWeapon(rangedWeaponSlot);
                }
                rangedWeaponSlot = newWeapon;
                Debug.Log($"✅ [PlayerSlotInventory] Stored {newWeapon.weaponName} in Ranged Slot.");
            }
        }

        //ForMeleeWeapon

        public void SwitchToMeleeWeapon()
        {
            if (meleeWeaponSlot != null)
            {
                playerInventory.EquipWeapon(meleeWeaponSlot);
                Debug.Log($"🔄 [PlayerSlotInventory] Switched to Melee: {meleeWeaponSlot.weaponName}");
            }
            else
            {
                Debug.Log("[PlayerSlotInventory] No melee weapon equipped.");
            }
        }
        
        //ForRangeWeapon

        public void SwitchToRangedWeapon()
        {
            if (rangedWeaponSlot != null)
            {
                playerInventory.EquipWeapon(rangedWeaponSlot);
                Debug.Log($"🔄 [PlayerSlotInventory] Switched to Ranged: {rangedWeaponSlot.weaponName}");
            }
            else
            {
                Debug.Log("[PlayerSlotInventory] No ranged weapon equipped.");
            }
        }
    }
}
