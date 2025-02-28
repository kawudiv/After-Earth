using Player.Components;
using UnityEngine;
using Weapons.Base;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private WeaponBase weaponPrefab; // ✅ The actual weapon object, assigned in Inspector

    public void PickupWeapon(PlayerInventory inventory)
    {
        if (weaponPrefab == null)
        {
            Debug.LogError("[ItemPickup] No weapon prefab assigned to this pickup!");
            return;
        }

        // ✅ Use the existing weapon instead of instantiating a new one
        WeaponBase newWeapon = weaponPrefab;

        // ✅ Equip the weapon through PlayerInventory
        inventory.EquipWeapon(newWeapon);

        Debug.Log($"✅ [ItemPickup] Picked up {newWeapon.weaponName}");

        // ✅ Disable the pickup object so it can't be picked up again
        gameObject.SetActive(false);
    }
}
