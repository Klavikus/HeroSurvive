using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class TargetFinderService : ITargetService
    {
        private readonly EnemyFactory _enemyFactory;
        
        private PlayerBuilder _playerBuilder;

        public TargetFinderService(EnemyFactory enemyFactory) => _enemyFactory = enemyFactory;

        public void BindPlayerBuilder(PlayerBuilder playerBuilder) => _playerBuilder = playerBuilder;

        public Vector3 GetPlayerPosition() =>
            _playerBuilder.GetPlayerPosition();

        public Vector3 GetPlayerDirection() =>
            _playerBuilder.GetPlayerDirection();

        public Vector3 GetClosestEnemyToPlayer() =>
            _enemyFactory.GetClosestEnemy(_playerBuilder.GetPlayerPosition());

        public Vector3 GetRandomEnemyPosition() =>
            _enemyFactory.GetRandomEnemyPosition();

        public Camera GetCamera() => _playerBuilder.GetPlayerCamera();
    }
}