using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Enums;
using GameCore.Source.Domain.Services;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace GameCore.Source.Controllers.Core.Factories
{
    public class EnemyFactory
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly Dictionary<EnemyType, EnemyData> _enemiesData;
        private readonly List<IEnemyController> _enemies;
        private readonly Dictionary<EnemyType, Queue<IEnemyController>> _enemyPoolByType;
        private readonly IVfxService _vfxService;
        private readonly IAudioPlayerService _audioPlayerService;

        public EnemyFactory(
            IConfigurationProvider configurationProvider,
            IVfxService vfxService,
            IAudioPlayerService audioPlayerService)
        {
            _configurationProvider = configurationProvider;
            _vfxService = vfxService;
            _audioPlayerService = audioPlayerService;

            _enemiesData = new Dictionary<EnemyType, EnemyData>();

            _enemyPoolByType = new Dictionary<EnemyType, Queue<IEnemyController>>();
            foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
                _enemyPoolByType.Add(enemyType, new Queue<IEnemyController>());

            foreach (EnemyData enemyData in _configurationProvider.EnemyConfig.EnemiesData)
                _enemiesData.Add(enemyData.Type, enemyData);

            _enemies = new List<IEnemyController>();
        }

        public IEnemyController Create(Vector3 at, EnemyType enemyType, ITargetService targetFinderService)
        {
            IEnemyController enemy = GetFromPool(enemyType);

            if (enemy == null)
            {
                enemy = Object.Instantiate(_enemiesData[enemyType].Prefab, at, Quaternion.identity)
                    .GetComponent<IEnemyController>();

                enemy.InvokedBackToPool += OnBackToPoolInvoked;
            }
            else
            {
                enemy.Transform.SetPositionAndRotation(at, Quaternion.identity);
            }

            _enemies.Add(enemy);

            enemy.Initialize(targetFinderService, _enemiesData[enemyType], _vfxService, _audioPlayerService);

            return enemy;
        }

        private IEnemyController GetFromPool(EnemyType enemyType)
        {
            if (_enemyPoolByType[enemyType].Count == 0)
                return null;

            return _enemyPoolByType[enemyType].Dequeue();
        }

        //TODO: Handle Died and Destroyed events
        private void OnBackToPoolInvoked(IEnemyController enemy)
        {
            _enemies.Remove(enemy);
            _enemyPoolByType[enemy.Type].Enqueue(enemy);
        }

        public Vector3 GetClosestEnemy(Vector3 to)
        {
            var livingEnemies = _enemies.Where(enemy => enemy.CanReceiveDamage).ToArray();

            if (livingEnemies.Length == 0)
                return Vector3.zero;

            float minDistance = Vector3.Distance(livingEnemies[0].Transform.position, to);
            int targetEnemyId = 0;

            for (var i = 0; i < livingEnemies.Length; i++)
            {
                if (livingEnemies[i].CanReceiveDamage == false)
                    continue;

                float newDistance = Vector3.Distance(livingEnemies[i].Transform.position, to);

                if (newDistance < minDistance)
                {
                    minDistance = newDistance;
                    targetEnemyId = i;
                }
            }

            return livingEnemies[targetEnemyId].Transform.position;
        }

        public Vector3 GetRandomEnemyPosition()
        {
            int enemiesCount = _enemies.Count;

            if (enemiesCount == 0)
                return Vector3.zero;

            return _enemies[Random.Range(0, enemiesCount)].Transform.position;
        }

        public void ClearEnemies()
        {
            foreach (IEnemyController enemy in _enemies)
                enemy?.Destroy();

            _enemies.Clear();
        }
    }
}