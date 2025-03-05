using UnityEngine;

namespace Items.Base
{
    public abstract class ItemBase : MonoBehaviour
    {
        [SerializeField]
        protected int itemID; // Unique identifier for the item

        [SerializeField]
        protected string itemName; // Human-readable name for the item

        public int ItemID => itemID; // Public getter for itemID
        public string ItemName => itemName; // Public getter for itemName

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            // Ensure itemID and itemName are initialized
            if (string.IsNullOrEmpty(itemName))
            {
                itemName = "Unnamed Item";
            }
        }

        public abstract void Use();
    }
}