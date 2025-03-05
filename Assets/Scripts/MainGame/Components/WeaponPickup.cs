using Player.Components;
using UnityEngine;
using Weapons.Base;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField]
    public WeaponBase weaponPrefab; // The weapon prefab to be picked up

    public void PickupWeapon(PlayerInventory inventory)
    {
        if (weaponPrefab == null)
        {
            Debug.LogError("[WeaponPickup] No weapon prefab assigned to this pickup!");
            return;
        }

        // Instantiate the weapon
        WeaponBase newWeapon = Instantiate(weaponPrefab);
        newWeapon.transform.position = transform.position;
        newWeapon.transform.rotation = transform.rotation;

        // Equip the weapon in the player's inventory
        inventory.EquipWeapon(newWeapon);

        Debug.Log($"✅ [WeaponPickup] Picked up {newWeapon.WeaponName}");

        // Disable the pickup object in the scene
        gameObject.SetActive(false);
    }

    internal WeaponBase GetWeaponPrefab()
    {
        return weaponPrefab;
    }
}