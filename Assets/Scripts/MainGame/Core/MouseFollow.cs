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
        private float enemyYOffset = 1f; // Y offset when hovering over an enemy

        private void Update()
        {
            if (mainCamera == null)
                return; // Prevents errors if camera isn't set

            // Get the mouse position in screen space
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // Create a ray from the camera through the mouse position
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            // Check if the ray hits an enemy
            if (DetectEnemy(ray, out Vector3 enemyTargetPosition))
            {
                // If an enemy is detected, use the enemy's position with an offset
                transform.position = enemyTargetPosition;
            }
            else
            {
                // If no enemy is detected, use the default aim behavior
                DefaultAim(ray);
            }
        }

        private void DefaultAim(Ray ray)
        {
            // Define a ground plane at the sphere's Y position
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

            // Check if the ray intersects with the ground plane
            if (groundPlane.Raycast(ray, out float enter))
            {
                // Get the point where the ray intersects the ground plane
                Vector3 targetPosition = ray.GetPoint(enter);

                // Apply the X and Z offsets
                targetPosition.x += offsetX;
                targetPosition.z += offsetZ;

                // Move the sphere to the target position
                transform.position = targetPosition;
            }
        }

        private bool DetectEnemy(Ray ray, out Vector3 enemyTargetPosition)
        {
            // Check if the ray hits an enemy
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, enemyLayer))
            {
                // If an enemy is detected, calculate the target position with an offset
                enemyTargetPosition = hit.point;
                enemyTargetPosition.y += enemyYOffset; // Apply Y offset
                return true;
            }

            // If no enemy is detected, return false
            enemyTargetPosition = Vector3.zero;
            return false;
        }
    }
}