using Player.Components;
using UnityEngine;
using Weapons.Base;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private WeaponBase weaponPrefab; // ✅ The weapon prefab to instantiate

    public void PickupWeapon(PlayerInventory inventory)
    {
        if (weaponPrefab == null)
        {
            Debug.LogError("[ItemPickup] No weapon prefab assigned to this pickup!");
            return;
        }

        // ✅ Instantiate a new weapon to avoid modifying the pickup object
        WeaponBase newWeapon = Instantiate(weaponPrefab);
        newWeapon.transform.position = transform.position;
        newWeapon.transform.rotation = transform.rotation;

        // ✅ Equip the weapon through PlayerInventory
        inventory.EquipWeapon(newWeapon);

        // ✅ Trigger the draw animation
        inventory.GetComponent<PlayerAnimation>()?.SetTrigger("DrawMelee");

        Debug.Log($"✅ [ItemPickup] Picked up {newWeapon.weaponName}");

        // ✅ Hide the pickup object to prevent duplicates
        gameObject.SetActive(false);
    }
}