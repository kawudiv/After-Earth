using Items.Base;
using UnityEngine;

namespace Items.Type
{
    public class QuestItem : ItemBase
    {
        public int Id;

        protected override void Awake()
        {
            base.Awake();
            itemID = Id; // Assign the ID from the Inspector
        }

        public override void Use()
        {
            Debug.Log($"{itemName} (ID: {itemID}) is Used!");
        }
    }
}
