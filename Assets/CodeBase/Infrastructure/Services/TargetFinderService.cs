using CodeBase.Infrastructure.Factories;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class TargetFinderService : ITargetService
    {
        private readonly PlayerBuilder _playerBuilder;
        private readonly EnemyFactory _enemyFactory;

        public TargetFinderService(PlayerBuilder playerBuilder, EnemyFactory enemyFactory)
        {
            _playerBuilder = playerBuilder;
            _enemyFactory = enemyFactory;
        }

        public Vector3 GetPlayerPosition() =>
            _playerBuilder.GetPlayerPosition();

        public Vector3 GetPlayerDirection() =>
            _playerBuilder.GetPlayerDirection();

        public Vector3 GetClosestEnemyToPlayer() =>
            _enemyFactory.GetClosestEnemy(_playerBuilder.GetPlayerPosition());

        public Vector3 GetRandomEnemyPosition() =>
            _enemyFactory.GetRandomEnemyPosition();
    }
}