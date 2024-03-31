using System;
using GameCore.Source.Controllers.Core.Factories;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Services
{
    public class TargetService : ITargetService
    {
        private readonly EnemyFactory _enemyFactory;
        private readonly PlayerModel _playerModel;
        private readonly RaycastHit2D[] _raycastHits = new RaycastHit2D[10];
        private PlayerBuilder _playerBuilder;

        // private PlayerBuilder _playerBuilder;

        public TargetService(
            EnemyFactory enemyFactory,
            PlayerModel playerModel)
        {
            _enemyFactory = enemyFactory ?? throw new ArgumentNullException(nameof(enemyFactory));
            _playerModel = playerModel ?? throw new ArgumentNullException(nameof(playerModel));
        }

        public void BindPlayerBuilder(PlayerBuilder playerBuilder) => _playerBuilder = playerBuilder;

        public Vector3 GetPlayerPosition() =>
            _playerModel.GetPlayerPosition();

        public Vector3 GetPlayerDirection() =>
            _playerModel.GetPlayerDirection();

        public Vector3 GetClosestEnemyToPlayer(float radius, LayerMask layerMask) =>
            _enemyFactory.GetClosestEnemy(_playerModel.GetPlayerPosition());

        public Vector3 GetRandomEnemyPosition() =>
            _enemyFactory.GetRandomEnemyPosition();

        public Camera GetCamera() => _playerModel.GetPlayerCamera();
    }
}