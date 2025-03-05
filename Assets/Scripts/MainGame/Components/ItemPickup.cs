using System;
using Player.Components;
using UnityEngine;
using Weapons.Base;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    public WeaponBase weaponPrefab; 

    public void PickupWeapon(PlayerInventory inventory)
    {
        if (weaponPrefab == null)
        {
            Debug.LogError("[ItemPickup] No weapon prefab assigned to this pickup!");
            return;
        }

        WeaponBase newWeapon = Instantiate(weaponPrefab);
        newWeapon.transform.position = transform.position;
        newWeapon.transform.rotation = transform.rotation;

        inventory.EquipWeapon(newWeapon);

        Debug.Log($"✅ [ItemPickup] Picked up {newWeapon.WeaponName}");

        gameObject.SetActive(false);
    }

    internal WeaponBase GetWeaponPrefab()
    {
        throw new NotImplementedException();
    }
}