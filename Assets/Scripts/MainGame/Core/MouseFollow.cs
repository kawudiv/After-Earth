using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class MouseFollow : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera; // Assign this in the Inspector

        [Header("Offset")]
        [SerializeField]
        private float offsetX = 0f; // Offset on the X axis

        [SerializeField]
        private float offsetZ = 0f; // Offset on the Z axis

        [Header("Enemy Detection")]
        [SerializeField]
        private LayerMask enemyLayer; // Assign the Enemy Layer in the Inspector

        [SerializeField]
        private float enemyYOffset = 0f; // Y offset when hovering over an enemy

        [Header("Player Reference")]
        [SerializeField]
        private Transform player; // Assign the Player in the Inspector

        [SerializeField]
        private float playerYOffset = 0f; // Offset applied to the player's Y position

        private void Update()
        {
            if (mainCamera == null || player == null)
                return; // Prevents errors if camera or player isn't set

            // Get the mouse position in screen space
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // Create a ray from the camera through the mouse position
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            // If enemy detected, follow it; otherwise, return to player height
            if (DetectEnemy(ray, out Vector3 enemyTargetPosition))
            {
                transform.position = enemyTargetPosition;
            }
            else
            {
                DefaultAim(ray);
            }
        }

        private void DefaultAim(Ray ray)
        {
            // Use the player's Y position with the offset
            float adjustedPlayerY = player.position.y + playerYOffset;

            // Define a ground plane at the adjusted player's Y position
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, adjustedPlayerY, 0));

            // Check if the ray intersects with the ground plane
            if (groundPlane.Raycast(ray, out float enter))
            {
                // Get the point where the ray intersects the ground plane
                Vector3 targetPosition = ray.GetPoint(enter);

                // Apply the X and Z offsets
                targetPosition.x += offsetX;
                targetPosition.z += offsetZ;

                // Apply the player's Y position with the offset
                targetPosition.y = adjustedPlayerY;

                // Move the cursor back to default position
                transform.position = targetPosition;
            }
        }

        private bool DetectEnemy(Ray ray, out Vector3 enemyTargetPosition)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, enemyLayer))
            {
                // If an enemy is detected, set position with an offset
                enemyTargetPosition = hit.point;
                enemyTargetPosition.y += enemyYOffset; // Apply Y offset
                return true;
            }

            enemyTargetPosition = Vector3.zero;
            return false;
        }
    }
}
