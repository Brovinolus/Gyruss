using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header ("Setting values")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private float delayBeforeNextStageMin = 2f;
    [SerializeField] private float delayBeforeNextStageMax = 4f;
    [SerializeField] private int minSpawnCount = 2;
    [SerializeField] private int maxSpawnCount = 10;

    private readonly List<GameObject> _enemies = new();
    private bool _isCoroutineRunning = false;
    
    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    public void EnemyDestroyed(GameObject enemy)
    {
        _enemies.Remove(enemy);
        
        if (_enemies.Count <= 0 && !_isCoroutineRunning)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    private IEnumerator SpawnEnemies()
    {
        _isCoroutineRunning = true;
        
        yield return new WaitForSeconds(Random.Range(delayBeforeNextStageMin, delayBeforeNextStageMax));
        
        var batchSize = Random.Range(minSpawnCount, maxSpawnCount + 1);
        
        var currentAngle = 360f / batchSize;
        var enemyType = Random.Range(0, enemyPrefabs.Count);
        
        for (var i = 0; i < batchSize; i++)
        {
            var spawnPosition = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));
            var instance = Instantiate(enemyPrefabs[enemyType], spawnPosition, Quaternion.identity);
            _enemies.Add(instance);
            currentAngle += 360f / batchSize;
        }

        _isCoroutineRunning = false;
        
        if (_enemies.Count <= 0)
        {
            StartCoroutine(SpawnEnemies());
        }
    }
}
