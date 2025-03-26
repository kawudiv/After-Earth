using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathUI : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;

    private void Awake()
    {
        deathScreen.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }
}
