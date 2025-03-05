using UnityEngine;

namespace Items.Base
{
    public abstract class ItemBase : MonoBehaviour
    {
        [SerializeField]
        protected int itemID; 

        [SerializeField]
        protected string itemName; 

        public int ItemID => itemID;
        public string ItemName => itemName; 

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