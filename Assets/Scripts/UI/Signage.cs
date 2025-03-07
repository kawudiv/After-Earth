using UnityEngine;
using Items.Type; // Ensure you are using the correct namespace

public class Signage : MonoBehaviour
{
    [SerializeField] private QuestItem requiredItem;
    [SerializeField] private GameObject signageUI;
    [SerializeField] private GameObject signageObject;
    private bool isCollected = false; // Local flag for whether the item is collected

    private void Start()
    {
        if (requiredItem != null && requiredItem.isCollected) // Use isCollected from QuestItem
        {
            HideSignage();
        }
        else
        {
            ShowSignage();
        }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, Camera.main.transform.position) < 5f)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isCollected)
                {
                    return;
                }
                else
                {
                    signageUI.SetActive(true);
                }
            }
        }
        else
        {
            signageUI.SetActive(false);
        }
    }

    public void CollectItem()
    {
        if (requiredItem != null && !isCollected)
        {
            isCollected = true;
            requiredItem.isCollected = true;  // Update the QuestItem's isCollected status
            HideSignage();
            Debug.Log("Signage is now Hidden");
        }
    }

    private void ShowSignage()
    {
        signageObject.SetActive(true);
    }

    private void HideSignage()
    {
        signageObject.SetActive(false);
    }
}
