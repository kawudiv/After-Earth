using UnityEngine;

namespace Items.Base
{
    public abstract class ItemBase : MonoBehaviour
    {
        [SerializeField]
        protected int itemID; // Unique ID for the item

        [SerializeField]
        protected string itemName; // Name of the item

        public int ItemID => itemID;
        public string ItemName => itemName;

        public Sprite ItemSprite; // Sprite for the item (optional)

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

        public abstract void Use(); // Abstract method for item usage
    }
}