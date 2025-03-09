using UnityEngine;
using Items.Type;

public class Signage : MonoBehaviour
{
    [SerializeField] private QuestItem requiredItem; // The item required to remove the signage
    [SerializeField] private GameObject signageUI; // The UI message to display
    [SerializeField] private GameObject signageObject; // The physical signage object
    private bool isPlayerNear = false; // Tracks if the player is near the signage

    private void Start()
    {
        // Initialize signage visibility based on whether the item is collected
        if (requiredItem != null && requiredItem.isCollected)
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
        // Check if the player is near the signage
        if (Vector3.Distance(transform.position, Camera.main.transform.position) < 5f)
        {
            isPlayerNear = true;

            // Check for player interaction
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (requiredItem != null && !requiredItem.isCollected)
                {
                    signageUI.SetActive(true); // Show the UI message
                }
            }
        }
        else
        {
            isPlayerNear = false;
            signageUI.SetActive(false); // Hide the UI message
        }
    }

    // Called when the required item is collected
    public void CollectItem()
    {
        if (requiredItem != null && !requiredItem.isCollected)
        {
            requiredItem.isCollected = true; // Mark the item as collected
            HideSignage(); // Hide the signage
            Debug.Log("Signage is now Hidden");
        }
    }

    // Show the physical signage object
    private void ShowSignage()
    {
        signageObject.SetActive(true);
    }

    // Hide the physical signage object
    private void HideSignage()
    {
        signageObject.SetActive(false);
    }
}