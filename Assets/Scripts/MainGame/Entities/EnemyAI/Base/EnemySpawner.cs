using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject meleeEnemyPrefab;  // Assign Melee Enemy prefab in Inspector
    public GameObject rangedEnemyPrefab; // Assign Ranged Enemy prefab in Inspector
    public GameObject bossEnemyPrefab;   // Assign Boss Enemy prefab in Inspector

    public Transform[] spawnPoints; // Assign spawn points in Inspector

    public float spawnInterval = 5f; // Time between spawns

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomSpawnIndex];

        int randomEnemyType = Random.Range(0, 3); // 0 = Melee, 1 = Ranged, 2 = Boss
        GameObject enemyPrefab = meleeEnemyPrefab;

        switch (randomEnemyType)
        {
            case 0:
                enemyPrefab = meleeEnemyPrefab;
                break;
            case 1:
                enemyPrefab = rangedEnemyPrefab;
                break;
            case 2:
                enemyPrefab = bossEnemyPrefab;
                break;
        }

        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}
