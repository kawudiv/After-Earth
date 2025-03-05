using Items.Base;
using Player.Components;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private ItemBase itemPrefab; 

    public void PickupItem(PlayerInventory inventory)
    {
        if (itemPrefab == null)
        {
            Debug.LogError("[ItemPickup] No item prefab assigned to this pickup!");
            return;
        }
        inventory.AddItem(itemPrefab);

        Debug.Log($"✅ [ItemPickup] Picked up {itemPrefab.ItemName}");

        gameObject.SetActive(false); 
    }

    internal ItemBase GetItemPrefab()
    {
        return itemPrefab;
    }
}
