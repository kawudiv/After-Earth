using UnityEngine;

namespace Core
{
    public class Billboard : MonoBehaviour
    {
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main; // ✅ Get the main camera
        }

        private void LateUpdate()
        {
            if (mainCamera == null)
                return;

            transform.LookAt(transform.position + mainCamera.transform.forward);
        }
    }
}
