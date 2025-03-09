using UnityEngine;

public class SignageUI : MonoBehaviour
{
    [SerializeField] private GameObject entranceUI; // The UI panel to display
    [SerializeField] private TMPro.TextMeshProUGUI messageText; // The text to display

    private void Start()
    {
        entranceUI.SetActive(false); // Hide the UI on start
    }

    // Show the UI message
    public void ShowEntranceMessage()
    {
        entranceUI.SetActive(true);
        messageText.text = "This entrance is not yet opened! Collect the item first!";
    }

    // Hide the UI message
    public void HideEntranceMessage()
    {
        entranceUI.SetActive(false);
    }
}