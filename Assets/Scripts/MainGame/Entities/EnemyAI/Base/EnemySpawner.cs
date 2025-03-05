using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _spawnContainer; // Add a container reference
    [SerializeField] private float _minimumSpawnTime = 1f;
    [SerializeField] private float _maximumSpawnTime = 3f;
    
    private float _timeUntilSpawn;

    void Awake()
    {
        SetTimeUntilSpawn();
    }

    void Update()
    {
        _timeUntilSpawn -= Time.deltaTime;

        if (_timeUntilSpawn <= 0)
        {
            SpawnEnemy();
            SetTimeUntilSpawn();
        }
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);

        // If a spawn container is set, parent the enemy to it
        if (_spawnContainer != null)
        {
            newEnemy.transform.SetParent(_spawnContainer);
        }
    }

    private void SetTimeUntilSpawn()
    {
        _timeUntilSpawn = Random.Range(_minimumSpawnTime, _maximumSpawnTime);
    }
}
