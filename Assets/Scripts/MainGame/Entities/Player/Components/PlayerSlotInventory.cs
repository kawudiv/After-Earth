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
            }
            else if (playerInventory.EquippedRangedWeapon != null)
            {
                playerInventory.DropWeapon(playerInventory.EquippedRangedWeapon);
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
            }
            else if (meleeWeaponSlot != null)
            {
                playerInventory.UnequipWeapon();
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
