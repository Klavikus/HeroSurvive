using Cinemachine;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.EntityComponents;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using UnityEngine;
using PlayerEventHandler = GameCore.Source.Controllers.Core.Services.PlayerEventHandler;

namespace GameCore.Source.Controllers.Core.Factories
{
    public class PlayerBuilder
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IPropertyProvider _propertyProvider;
        private readonly IModelProvider _modelProvider;
        private readonly IAbilityUpgradeService _abilityUpgradeService;
        private readonly AbilityFactory _abilityFactory;
        private readonly IAudioPlayerService _audioPlayerService;

        private PlayerController _playerController;

        private Vector3 _playerPosition;
        private Vector3 _playerDirection;
        private GameObject _playerPrefab;
        private InputController _inputController;
        private MoveController _moveController;
        private CinemachineVirtualCamera _currentCamera;
        private Camera _playerCamera;

        public PlayerBuilder(
            IModelProvider modelProvider,
            IConfigurationProvider configurationProvider,
            IPropertyProvider propertyProvider,
            IAbilityUpgradeService abilityUpgradeService,
            AbilityFactory abilityFactory,
            IAudioPlayerService audioPlayerService)
        {
            _modelProvider = modelProvider;
            _configurationProvider = configurationProvider;
            _propertyProvider = propertyProvider;
            _abilityUpgradeService = abilityUpgradeService;
            _abilityFactory = abilityFactory;
            _audioPlayerService = audioPlayerService;
        }

        public PlayerController Build(AbilityConfigSO initialAbilityConfigSO)
        {
            _playerController = GameObject.Instantiate(_modelProvider.Get<HeroModel>().CurrentSelectedHero.Prefab,
                Vector3.zero,
                Quaternion.identity).GetComponent<PlayerController>();
            _inputController = _playerController.GetComponent<InputController>();
            _inputController.Initialize();
            _moveController = _playerController.GetComponent<MoveController>();
            _inputController.InputUpdated += OnPlayerInputUpdated;
            _playerController.Initialize(_propertyProvider, initialAbilityConfigSO, _abilityFactory,
                _audioPlayerService);

            PlayerModel playerModel = new PlayerModel()
            {
                AbilityContainer = _playerController.AbilityContainer,
                IsFreeSlotAvailable = _playerController.IsFreeSlotAvailable
            };
            
            _abilityUpgradeService.BindToPlayer(playerModel);

            return _playerController;
        }

        private void OnPlayerInputUpdated(InputData inputData)
        {
            Vector3 newDirection = new Vector3(inputData.Horizontal, inputData.Vertical, 0);

            if (newDirection.sqrMagnitude <= 0.3f)
                return;

            _playerDirection = newDirection;
        }

        public IAbilityHandler GetPlayerAbilityHandler() =>
            _playerController.GetComponentInChildren<IAbilityHandler>();

        public Vector3 GetPlayerPosition()
        {
            if (_playerController)
                _playerPosition = _playerController.transform.position;

            return _playerPosition;
        }

        public Vector3 GetPlayerDirection() =>
            _moveController.LastMoveVector;

        public void BindCameraToPlayer()
        {
            _currentCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
            _currentCamera.Follow = _playerController.transform;
            _playerCamera = GameObject.FindObjectOfType<Camera>();
        }

        public void BindEventsHandler(PlayerEventHandler playerEventHandler)
        {
            IDamageable damagable = _playerController.GetComponent<IDamageable>();
            playerEventHandler.Initialize(damagable);
        }

        public void RespawnPlayer()
        {
            if (_playerController == null)
                return;

            _playerController.GetComponent<IDamageable>().Respawn();
        }

        public Camera GetPlayerCamera() => _playerCamera;
    }
}