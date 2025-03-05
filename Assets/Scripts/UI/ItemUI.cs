using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] public Image UnknownItem;
    [SerializeField] public Image ItemReveal;
    public Sprite newItemSprite;  

    public void UpdateItemUI(Sprite itemSprite)
    {
        // Debug checks for null references
        if (UnknownItem == null)
        {
            Debug.LogError("First image is not assigned in ItemUI!");
        }
        if (ItemReveal == null)
        {
            Debug.LogError("Second image is not assigned in ItemUI!");
        }

        if (itemSprite != null)
        {
            UnknownItem.enabled = false;
            ItemReveal.enabled = true;
            ItemReveal.sprite = itemSprite;
        }
        else
        {
            UnknownItem.enabled = true;
            ItemReveal.enabled = false;
        }
    }
}
