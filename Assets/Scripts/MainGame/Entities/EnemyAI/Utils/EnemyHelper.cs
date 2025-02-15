using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI.Utils
{
    public static class EnemyHelper
    {
        /// <summary>
        /// Checks if a player is within a certain range.
        /// </summary>
        public static bool IsPlayerInRange(Transform enemy, Transform player, float range)
        {
            return Vector3.Distance(enemy.position, player.position) <= range;
        }

        /// <summary>
        /// Finds a random point within a given range on the NavMesh.
        /// </summary>
        public static Vector3 GetRandomNavMeshPoint(Vector3 origin, float radius, int maxAttempts = 5)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * radius;
                randomDirection.y = 0;
                Vector3 finalPosition = origin + randomDirection;

                if (NavMesh.SamplePosition(finalPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return origin; // Fallback to original position
        }

        /// <summary>
        /// Rotates an enemy to smoothly look at the player.
        /// </summary>
        public static void RotateTowards(Transform enemy, Transform target, float rotationSpeed)
        {
            Vector3 direction = (target.position - enemy.position).normalized;
            direction.y = 0; // Keep rotation level

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                enemy.rotation = Quaternion.Slerp(enemy.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
