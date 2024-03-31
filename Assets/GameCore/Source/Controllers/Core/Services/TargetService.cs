using GameCore.Source.Controllers.Core.Factories;
using GameCore.Source.Domain.Services;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Services
{
    public class TargetService : ITargetService
    {
        private readonly EnemyFactory _enemyFactory;
        private readonly RaycastHit2D[] _raycastHits = new RaycastHit2D[10];

        private PlayerBuilder _playerBuilder;

        public TargetService(EnemyFactory enemyFactory) => _enemyFactory = enemyFactory;

        public void BindPlayerBuilder(PlayerBuilder playerBuilder) => _playerBuilder = playerBuilder;

        public Vector3 GetPlayerPosition() =>
            _playerBuilder.GetPlayerPosition();

        public Vector3 GetPlayerDirection() =>
            _playerBuilder.GetPlayerDirection();

        public Vector3 GetClosestEnemyToPlayer(float radius, LayerMask layerMask) => 
            _enemyFactory.GetClosestEnemy(_playerBuilder.GetPlayerPosition());

        public Vector3 GetRandomEnemyPosition() =>
            _enemyFactory.GetRandomEnemyPosition();

        public Camera GetCamera() => _playerBuilder.GetPlayerCamera();
    }
}