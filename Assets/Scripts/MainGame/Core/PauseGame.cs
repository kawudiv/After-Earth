using UnityEngine;

namespace Core
{
    public class PauseGame : MonoBehaviour
    {
        private bool isPaused = false;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P)) // Change 'P' to any key you want
            {
                TogglePause();
            }
        }

        void TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            // Optionally, show/hide a pause menu UI here
        }
    }
}
