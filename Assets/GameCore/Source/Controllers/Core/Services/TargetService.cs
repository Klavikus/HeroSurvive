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
        private PlayerFactory _playerFactory;

        // private PlayerBuilder _playerBuilder;

        public TargetService(
            EnemyFactory enemyFactory,
            PlayerModel playerModel)
        {
            _enemyFactory = enemyFactory ?? throw new ArgumentNullException(nameof(enemyFactory));
            _playerModel = playerModel ?? throw new ArgumentNullException(nameof(playerModel));
        }

        public void BindPlayerBuilder(PlayerFactory playerFactory) => _playerFactory = playerFactory;

        public Vector3 GetPlayerPosition() =>
            _playerModel.GetPosition();

        public Vector3 GetPlayerDirection() =>
            _playerModel.GetDirection();

        public Vector3 GetClosestEnemyToPlayer(float radius, LayerMask layerMask) =>
            _enemyFactory.GetClosestEnemy(_playerModel.GetPosition());

        public Vector3 GetRandomEnemyPosition() =>
            _enemyFactory.GetRandomEnemyPosition();

        public Camera GetCamera() => _playerModel.GetCamera();
    }
}