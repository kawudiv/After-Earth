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
                    pickup.gameObject.SetActive(false);
                    return;
                }
            }
            Debug.Log("[PlayerInventory] No weapon found to pick up.");
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

            Debug.Log($"✅ [PlayerInventory] Dropped {weaponToDrop.weaponName}");

            playerAnimation?.SetTrigger("DropWeapon");
            WeaponDrawnToggle(false);

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