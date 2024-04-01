using Cinemachine;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.EntityComponents;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Factories
{
    public class PlayerFactory
    {
        private readonly HeroModel _heroModel;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IPropertyProvider _propertyProvider;
        private readonly IAbilityUpgradeService _abilityUpgradeService;
        private readonly AbilityFactory _abilityFactory;
        private readonly IAudioPlayerService _audioPlayerService;
        private readonly PlayerModel _playerModel;

        private PlayerController _playerController;

        private GameObject _playerPrefab;
        private InputController _inputController;
        private MoveController _moveController;
        private CinemachineVirtualCamera _currentCamera;
        private Camera _playerCamera;

        public PlayerFactory(
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

        public PlayerController Create(IGameLoopService gameLoopService)
        {
            AbilityConfigSO initialAbilityConfigSo = _heroModel.CurrentSelectedHero.InitialAbilityConfig;
            _playerController = Object.Instantiate(_heroModel.CurrentSelectedHero.Prefab,
                Vector3.zero,
                Quaternion.identity).GetComponent<PlayerController>();
            _inputController = _playerController.GetComponent<InputController>();
            _inputController.Initialize();
            _moveController = _playerController.GetComponent<MoveController>();
            _playerController.Initialize(_propertyProvider, initialAbilityConfigSo, _abilityFactory,
                _audioPlayerService, gameLoopService);

            _playerModel.AbilityContainer = _playerController.AbilityContainer;
            _playerModel.IsFreeSlotAvailable = _playerController.IsFreeSlotAvailable;
            _playerModel.Transform = _playerController.transform;
            _playerModel.MoveController = _moveController;
            _playerModel.Camera = _playerCamera;

            _abilityUpgradeService.BindToPlayer(_playerModel);

            BindCameraToPlayer();

            return _playerController;
        }

        private void BindCameraToPlayer()
        {
            _currentCamera = Object.FindObjectOfType<CinemachineVirtualCamera>();
            _currentCamera.Follow = _playerController.transform;
            _playerCamera = Object.FindObjectOfType<Camera>();
            _playerModel.Camera = _playerCamera;
        }
    }
}