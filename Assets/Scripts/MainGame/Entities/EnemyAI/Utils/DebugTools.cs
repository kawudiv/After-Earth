using UnityEngine;

namespace EnemyAI.Utils
{
    public static class DebugTools
    {
        /// <summary>
        /// Draws a debug sphere to visualize detection range.
        /// </summary>
        public static void DrawDetectionRange(Vector3 position, float range, Color color)
        {
            #if UNITY_EDITOR
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.DrawWireDisc(position, Vector3.up, range);
            #endif
        }

        /// <summary>
        /// Logs enemy state changes.
        /// </summary>
        public static void LogStateChange(string enemyName, string newState)
        {
            Debug.Log($"[EnemyAI] {enemyName} switched to {newState} state.");
        }

        /// <summary>
        /// Displays a warning if AI gets stuck.
        /// </summary>
        public static void CheckStuck(Transform enemy, Vector3 lastPosition, ref float stuckTimer, float threshold = 1f)
        {
            if (Vector3.Distance(enemy.position, lastPosition) < 0.1f)
            {
                stuckTimer += Time.deltaTime;
                if (stuckTimer >= threshold)
                {
                    Debug.LogWarning($"[EnemyAI] {enemy.name} might be stuck!");
                }
            }
            else
            {
                stuckTimer = 0; // Reset if moving
            }
        }
    }
}
