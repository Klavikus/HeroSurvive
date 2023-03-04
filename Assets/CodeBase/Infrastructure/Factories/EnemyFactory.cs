using System.Collections.Generic;
using System.Linq;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enemies;
using CodeBase.Domain.Enums;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Infrastructure.Factories
{
    public class EnemyFactory
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly Dictionary<EnemyType, EnemyData> _enemiesData;
        private GameObject _enemyPrefab;
        private List<Enemy> _enemies;

        public EnemyFactory(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _enemiesData = new Dictionary<EnemyType, EnemyData>();
            foreach (EnemyData enemyData in _configurationProvider.EnemyConfig.EnemiesData)
                _enemiesData.Add(enemyData.Type, enemyData);

            _enemies = new List<Enemy>();
        }

        public Enemy Create(Vector3 at, EnemyType enemyType, ITargetService targetFinderService)
        {
            Enemy enemy = GameObject.Instantiate(_enemiesData[enemyType].Prefab, at, Quaternion.identity);
            _enemies.Add(enemy);
            enemy.Destroyed += OnEnemyDied;
            enemy.Initialize(targetFinderService, _enemiesData[enemyType]);
            return enemy;
        }

        //TODO: Handle Died and Destroyed events
        private void OnEnemyDied(Enemy enemyAI)
        {
            _enemies.Remove(enemyAI);
            enemyAI.Destroyed -= OnEnemyDied;
        }

        public Vector3 GetClosestEnemy(Vector3 to)
        {
            if (_enemies.Count == 0)
                return Vector3.zero;

            float minDistance = Vector3.Distance(_enemies[0].transform.position, to);
            int targetEnemyId = 0;

            for (var i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].CanReceiveDamage == false)
                    continue;

                float newDistance = Vector3.Distance(_enemies[i].transform.position, to);

                if (newDistance < minDistance)
                {
                    minDistance = newDistance;
                    targetEnemyId = i;
                }
            }

            return _enemies[targetEnemyId].transform.position;
        }

        public Vector3 GetRandomEnemyPosition()
        {
            Enemy[] livingEnemy = _enemies.Where(enemy => enemy.CanReceiveDamage).ToArray();

            if (livingEnemy.Length == 0)
                return Vector3.zero;

            return livingEnemy[Random.Range(0, livingEnemy.Length)].transform.position;
        }

        public void ClearEnemies()
        {
            foreach (Enemy enemy in _enemies)
            {
                GameObject.Destroy(enemy);
            }
        }
    }
}