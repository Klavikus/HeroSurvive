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
        private readonly HeroModel _heroModel;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IPropertyProvider _propertyProvider;
        private readonly IAbilityUpgradeService _abilityUpgradeService;
        private readonly AbilityFactory _abilityFactory;
        private readonly IAudioPlayerService _audioPlayerService;
        private readonly PlayerModel _playerModel;

        private PlayerController _playerController;

        private Vector3 _playerPosition;
        private Vector3 _playerDirection;
        private GameObject _playerPrefab;
        private InputController _inputController;
        private MoveController _moveController;
        private CinemachineVirtualCamera _currentCamera;
        private Camera _playerCamera;

        public PlayerBuilder(
            HeroModel heroModel,
            IConfigurationProvider configurationProvider,
            IPropertyProvider propertyProvider,
            IAbilityUpgradeService abilityUpgradeService,
            AbilityFactory abilityFactory,
            IAudioPlayerService audioPlayerService,
            PlayerModel playerModel)
        {
            _heroModel = heroModel;
            _configurationProvider = configurationProvider;
            _propertyProvider = propertyProvider;
            _abilityUpgradeService = abilityUpgradeService;
            _abilityFactory = abilityFactory;
            _audioPlayerService = audioPlayerService;
            _playerModel = playerModel;
        }

        public PlayerController Build(AbilityConfigSO initialAbilityConfigSO, IGameLoopService gameLoopService)
        {
            _playerController = Object.Instantiate(_heroModel.CurrentSelectedHero.Prefab,
                Vector3.zero,
                Quaternion.identity).GetComponent<PlayerController>();
            _inputController = _playerController.GetComponent<InputController>();
            _inputController.Initialize();
            _moveController = _playerController.GetComponent<MoveController>();
            _inputController.InputUpdated += OnPlayerInputUpdated;
            _playerController.Initialize(_propertyProvider, initialAbilityConfigSO, _abilityFactory,
                _audioPlayerService, gameLoopService);

            _playerModel.AbilityContainer = _playerController.AbilityContainer;
            _playerModel.IsFreeSlotAvailable = _playerController.IsFreeSlotAvailable;
            _playerModel.Transform = _playerController.transform;
            _playerModel.MoveController = _moveController;
            _playerModel.Camera = _playerCamera;

            _abilityUpgradeService.BindToPlayer(_playerModel);

            return _playerController;
        }

        private void OnPlayerInputUpdated(InputData inputData)
        {
            Vector3 newDirection = new Vector3(inputData.Horizontal, inputData.Vertical, 0);

            if (newDirection.sqrMagnitude <= 0.3f)
                return;

            _playerDirection = newDirection;
        }

        public void BindCameraToPlayer()
        {
            _currentCamera = Object.FindObjectOfType<CinemachineVirtualCamera>();
            _currentCamera.Follow = _playerController.transform;
            _playerCamera = Object.FindObjectOfType<Camera>();
            _playerModel.Camera = _playerCamera;
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
    }
}