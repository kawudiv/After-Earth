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
            itemID = Id;
        }

        public override void Use()
        {
            Debug.Log($"{itemName} (ID: {itemID}) is Used!");
        }
    }
}
