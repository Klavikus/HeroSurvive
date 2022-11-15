using System.Collections;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.ForSort
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float _circleRadius;
        [SerializeField] private float _spawnDelay;
        [SerializeField] private int _enemyInWave;

        private EnemySpawnService _enemySpawnService;

        public void Initialize(EnemySpawnService enemySpawnService)
        {
            _enemySpawnService = enemySpawnService;
        }
    }
}