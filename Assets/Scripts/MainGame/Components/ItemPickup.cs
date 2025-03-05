using Items.Base;
using Player.Components;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private ItemBase itemPrefab; // The item prefab to be picked up

    public void PickupItem(PlayerInventory inventory)
    {
        if (itemPrefab == null)
        {
            Debug.LogError("[ItemPickup] No item prefab assigned to this pickup!");
            return;
        }

        // Instantiate the item
        ItemBase newItem = Instantiate(itemPrefab);
        newItem.transform.position = transform.position;
        newItem.transform.rotation = transform.rotation;

        // Add the item to the player's inventory
        inventory.AddItem(newItem);

        Debug.Log($"✅ [ItemPickup] Picked up {newItem.ItemName}");

        // Disable the pickup object in the scene
        newItem.gameObject.SetActive(false);
    }

    internal ItemBase GetItemPrefab()
    {
        return itemPrefab;
    }
}