using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Domain;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Infrastructure
{
    public class EnemyFactory
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly Dictionary<EnemyType, EnemyData> _enemiesData;
        private GameObject _enemyPrefab;
        private List<Enemy> _enemies;

        private readonly Dictionary<EnemyType, Queue<Enemy>> _enemyPoolByType;

        public EnemyFactory(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _enemiesData = new Dictionary<EnemyType, EnemyData>();

            _enemyPoolByType = new Dictionary<EnemyType, Queue<Enemy>>();
            foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
                _enemyPoolByType.Add(enemyType, new Queue<Enemy>());

            foreach (EnemyData enemyData in _configurationProvider.EnemyConfig.EnemiesData)
                _enemiesData.Add(enemyData.Type, enemyData);

            _enemies = new List<Enemy>();
        }

        public Enemy Create(Vector3 at, EnemyType enemyType, ITargetService targetFinderService)
        {
            Enemy enemy = GetFromPool(enemyType);

            if (enemy == null)
            {
                enemy = GameObject.Instantiate(_enemiesData[enemyType].Prefab, at, Quaternion.identity);
                _enemies.Add(enemy);
                enemy.InvokedBackToPool += OnBackToPoolInvoked;
            }
            else
            {
                enemy.transform.SetPositionAndRotation(at, Quaternion.identity);
            }

            enemy.Initialize(targetFinderService, _enemiesData[enemyType]);
            return enemy;
        }

        private Enemy GetFromPool(EnemyType enemyType)
        {
            if (_enemyPoolByType[enemyType].Count == 0)
                return null;

            return _enemyPoolByType[enemyType].Dequeue();
        }

        //TODO: Handle Died and Destroyed events
        private void OnBackToPoolInvoked(Enemy enemy)
        {
            _enemyPoolByType[enemy.Type].Enqueue(enemy);
        }

        public Vector3 GetClosestEnemy(Vector3 to)
        {
            var livingEnemies = _enemies.Where(enemy => enemy.CanReceiveDamage).ToArray();
            if (livingEnemies.Length == 0)
                return Vector3.zero;

            float minDistance = Vector3.Distance(livingEnemies[0].transform.position, to);
            int targetEnemyId = 0;

            for (var i = 0; i < livingEnemies.Length; i++)
            {
                if (livingEnemies[i].CanReceiveDamage == false)
                    continue;

                float newDistance = Vector3.Distance(livingEnemies[i].transform.position, to);

                if (newDistance < minDistance)
                {
                    minDistance = newDistance;
                    targetEnemyId = i;
                }
            }

            return livingEnemies[targetEnemyId].transform.position;
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
                GameObject.Destroy(enemy);

            _enemies.Clear();
        }
    }
}