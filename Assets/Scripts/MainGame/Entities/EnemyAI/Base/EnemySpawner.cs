using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject meleeEnemyPrefab;
    public GameObject rangedEnemyPrefab;
    public GameObject bossEnemyPrefab;  

    public Transform[] spawnPoints; 

    public float spawnInterval = 5f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomSpawnIndex];

        int randomEnemyType = Random.Range(0, 3); 
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
