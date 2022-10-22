using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _circleRadius;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private int _enemyInWave;

    private EnemySpawnService _enemySpawnService;

    public void Initialize(EnemySpawnService enemySpawnService)
    {
        _enemySpawnService = enemySpawnService;

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        while (true)
        {
            _enemySpawnService.SpawnWaveInCircle(_enemyInWave, _circleRadius);
            yield return new WaitForSeconds(_spawnDelay);
        }
    }
}