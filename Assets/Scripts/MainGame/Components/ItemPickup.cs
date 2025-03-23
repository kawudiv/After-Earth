using Items.Base;
using Player.Components;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemBase itemPrefab;
    [SerializeField] private Signage signage; 

    public void PickupItem(PlayerInventory inventory)
    {
        if (itemPrefab == null)
        {
            Debug.LogError("[ItemPickup] No Item prefab assigned to this pickup");
            return;
        }

        inventory.AddItem(itemPrefab);

        Debug.Log($"✅[ItemPickup] Picked Up {itemPrefab.ItemName}");

        if (signage != null)
        {
            signage.CollectItem();
        }

        gameObject.SetActive(false);
    }

    internal ItemBase GetItemPrefab()
    {
        return itemPrefab;
    }
}