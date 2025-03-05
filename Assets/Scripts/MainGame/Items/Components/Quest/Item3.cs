using Items.Base;
using Items.Type;
using UnityEngine;

namespace Items.Components.Quest
{
    public class Item3 : QuestItem
    {
        protected override void Awake()
        {
            base.Awake();
            itemID = 3; // Set a specific ID for Item1
            itemName = "Item3"; // Set a specific name for Item1
        }

        public override void Use()
        {
            base.Use();
            Debug.Log($"{itemName} (ID: {itemID}) is indeed being used!");
        }
    }
}
