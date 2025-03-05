using Items.Base;
using Items.Type;
using UnityEngine;

namespace Items.Components.Quest
{
    public class Item1 : QuestItem
    {
        protected override void Awake()
        {
            base.Awake();
            itemID = 0; // Set a specific ID for Item1
            itemName = "Item1"; // Set a specific name for Item1
        }

        public override void Use()
        {
            base.Use();
            Debug.Log($"{itemName} (ID: {itemID}) is indeed being used!");
        }
    }
}
