using EnemyAI.Base;
using Player.Base;
using UnityEngine;

namespace EnemyAI.Components
{
    public class EnemySensor : MonoBehaviour
    {
        [Header("Detection Settings")]
        [SerializeField]
        public LayerMask layers = ~0; // Default to "Everything"

        [SerializeField]
        public LayerMask occlusionLayers = 1; // Default to "Default"

        [Header("Debug Settings")]
        public bool showDebug = true; // Toggle debug visuals
        public Color detectionRangeColor = Color.yellow; // Color for detection range sphere
        public Color lineOfSightColor = Color.blue; // Color for unobstructed line of sight
        public Color obstructedColor = Color.red; // Color for obstructed line of sight

        private Transform target; // Reference to the player
        private EnemyBase enemyBase;

        private void Awake()
        {
            // Assign EnemyBase component
            enemyBase = GetComponent<EnemyBase>();
            if (enemyBase == null)
            {
                Debug.LogWarning("[EnemySensor] EnemyBase component is missing!", this);
            }

            // Find the player by tag
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
            else
            {
                Debug.LogWarning(
                    "[EnemySensor] No GameObject with tag 'Player' found in the scene!",
                    this
                );
            }
        }

        /// <summary>
        /// Checks if the player is visible based on distance and occlusion.
        /// </summary>
        public bool CanSeePlayer()
        {
            if (target == null || enemyBase == null)
                return false;

            // Check if the player is within detection range
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer > enemyBase.detectionRange)
                return false; // Player is out of range

            // Check if player is in direct line of sight
            return IsInSight();
        }

        /// <summary>
        /// Checks if the player is visible or obstructed by a wall (layer Default).
        /// </summary>
        private bool IsInSight()
        {
            if (target == null)
                return false;

            // Perform a linecast to check if there's an obstacle between enemy and player
            if (
                Physics.Linecast(
                    transform.position,
                    target.position,
                    out RaycastHit hit,
                    occlusionLayers
                )
            )
            {
                GameObject hitObject = hit.collider.gameObject;

                // Allow objects with the "Enemy" tag and "Enemy" layer to pass through
                if (
                    hitObject.CompareTag("Enemy")
                    && hitObject.layer == LayerMask.NameToLayer("Enemy")
                )
                {
                    return true;
                }
                if (showDebug)
                {
                    Debug.Log(
                        "[EnemySensor] Player is obstructed by " + hit.collider.gameObject.name
                    );
                    Debug.DrawLine(transform.position, hit.point, obstructedColor, 1f);
                    Debug.DrawLine(hit.point, target.position, Color.green, 1f); // Show obstruction point
                }
                return false; // Player is behind an obstacle
            }

            if (showDebug)
            {
                Debug.Log("[EnemySensor] Player is in sight.");
                Debug.DrawLine(transform.position, target.position, lineOfSightColor, 1f);
            }
            return true; // Player is visible
        }

        private void OnDrawGizmos()
        {
            if (!showDebug)
                return;

            if (enemyBase == null)
            {
                enemyBase = GetComponent<EnemyBase>(); // Try to assign dynamically
                if (enemyBase == null)
                    return; // Avoid NullReferenceException
            }

            // Draw detection range
            Gizmos.color = detectionRangeColor;
            Gizmos.DrawWireSphere(transform.position, enemyBase.detectionRange);

            // Draw line of sight if target is assigned
            if (target != null)
            {
                Gizmos.color = CanSeePlayer() ? lineOfSightColor : obstructedColor;
                Gizmos.DrawLine(transform.position, target.position);
            }
        }
    }
}
