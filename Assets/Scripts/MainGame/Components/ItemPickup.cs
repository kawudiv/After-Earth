using Items.Base;
using Player.Components;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemBase itemPrefab; // The item to be picked up
    [SerializeField] private Signage signage; // The signage that blocks the entrance

    public void PickupItem(PlayerInventory inventory)
    {
        if (itemPrefab == null)
        {
            Debug.LogError("[ItemPickup] No Item prefab assigned to this pickup");
            return;
        }

        // Add the item to the player's inventory
        inventory.AddItem(itemPrefab);

        Debug.Log($"✅[ItemPickup] Picked Up {itemPrefab.ItemName}");

        // Notify the signage that the item has been collected
        if (signage != null)
        {
            signage.CollectItem();
        }

        // Disable the item in the scene after pickup
        gameObject.SetActive(false);
    }

    internal ItemBase GetItemPrefab()
    {
        return itemPrefab;
    }
}