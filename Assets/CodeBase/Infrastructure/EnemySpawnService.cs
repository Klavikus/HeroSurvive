using System;
using System.Collections.Generic;
using CodeBase.Domain.Enemies;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Infrastructure
{
    public interface IEnemySpawnService : IService
    {
        Enemy[] SpawnWave(EnemySpawnData[] enemiesSpawnData);
        void MoveCloserToPlayer(Enemy enemy);
        void ClearEnemies();
    }

    public class EnemySpawnService : IEnemySpawnService
    {
        private readonly TargetFinderService _targetFinderService;
        private readonly EnemyFactory _enemyFactory;

        public EnemySpawnService(TargetFinderService targetFinderService, EnemyFactory enemyFactory)
        {
            _targetFinderService = targetFinderService;
            _enemyFactory = enemyFactory;
        }

        public Enemy[] SpawnWave(EnemySpawnData[] enemiesSpawnData)
        {
            List<Enemy> result = new List<Enemy>();

            foreach (EnemySpawnData spawnData in enemiesSpawnData)
            {
                for (int i = 0; i < spawnData.Count; i++)
                {
                    Vector3 spawnPosition = GetRandomAvailablePosition();
                    Enemy enemy = _enemyFactory.Create(at: spawnPosition, spawnData.EnemyType, _targetFinderService);
                    result.Add(enemy);
                }
            }

            return result.ToArray();
        }

        public void MoveCloserToPlayer(Enemy enemy)
        {
            enemy.transform.position = GetRandomAvailablePosition();
        }

        public void ClearEnemies()
        {
            _enemyFactory.ClearEnemies();
        }

        private Vector3 GetRandomAvailablePosition()
        {
            Vector2 xRange = new Vector2(-15, 15);
            Vector2 yRange = new Vector2(-15, 15);
            Vector2 playerPosition = _targetFinderService.GetPlayerPosition();

            return new Vector3(playerPosition.x + Random.Range(xRange.x, xRange.y),
                playerPosition.y + Random.Range(yRange.x, yRange.y), 0);
        }
    }
}