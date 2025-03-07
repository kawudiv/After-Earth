using UnityEngine;

public class SignageUI : MonoBehaviour
{
    [SerializeField] private GameObject entranceUI; // UI object to notify the player
    [SerializeField] private TMPro.TextMeshProUGUI messageText; // Text element to display message

    private void Start()
    {
        entranceUI.SetActive(false); // Hide the UI by default
    }

    public void ShowEntranceMessage()
    {
        entranceUI.SetActive(true);
        messageText.text = "This entrance is not yet opened! Collect the item first!";
    }

    public void HideEntranceMessage()
    {
        entranceUI.SetActive(false);
    }
}
