using Cinemachine;
using CodeBase.Domain.Abilities;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enemies;
using CodeBase.ForSort;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PropertiesProviders;
using CodeBase.MVVM.Models;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public class PlayerBuilder
    {
        private readonly ConfigurationProvider _configurationProvider;
        private readonly IPropertyProvider _propertyProvider;
        private readonly HeroModel _heroModel;

        private Player _player;

        private Vector3 _playerPosition;
        private Vector3 _playerDirection;
        private GameObject _playerPrefab;
        private InputController _inputController;
        private MoveController _moveController;

        public PlayerBuilder(HeroModel heroModel,
            ConfigurationProvider configurationProvider, IPropertyProvider propertyProvider)
        {
            _heroModel = heroModel;
            _configurationProvider = configurationProvider;
            _propertyProvider = propertyProvider;
        }

        public Player Build(Ability initialAbility)
        {
            _player = GameObject.Instantiate(_heroModel.CurrentSelectedHero.Prefab, Vector3.zero, Quaternion.identity);
            _inputController = _player.GetComponent<InputController>();
            _moveController = _player.GetComponent<MoveController>();
            _inputController.InputUpdated += OnPlayerInputUpdated;
            _player.Initialize(_propertyProvider, initialAbility);
            return _player;
        }

        private void OnPlayerInputUpdated(InputData inputData)
        {
            Vector3 newDirection = new Vector3(inputData.Horizontal, inputData.Vertical, 0);

            if (newDirection.sqrMagnitude <= 0.3f)
                return;

            _playerDirection = newDirection;
        }

        public IAbilityHandler GetPlayerAbilityHandler() =>
            _player.GetComponentInChildren<IAbilityHandler>();

        public Vector3 GetPlayerPosition()
        {
            if (_player)
                _playerPosition = _player.transform.position;

            return _playerPosition;
        }

        public Vector3 GetPlayerDirection() =>
            _moveController.LastMoveVector;

        public void BindCameraToPlayer() =>
            GameObject.FindObjectOfType<CinemachineVirtualCamera>().Follow = _player.transform;

        public void BindEventsHandler(PlayerEventHandler playerEventHandler)
        {
            IDamageable damagable = _player.GetComponent<IDamageable>();
            playerEventHandler.Initialize(damagable);
        }
    }
}