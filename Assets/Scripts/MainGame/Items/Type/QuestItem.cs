using Items.Base;
using UnityEngine;

namespace Items.Type
{
    public class QuestItem : ItemBase
    {
        public int Id; // Unique ID for the quest item
        public bool isCollected; // Tracks if the item has been collected

        protected override void Awake()
        {
            base.Awake();
            itemID = Id; // Set the item ID
        }

        public override void Use()
        {
            Debug.Log($"{itemName} (ID: {itemID}) is Used!");
        }
    }
}