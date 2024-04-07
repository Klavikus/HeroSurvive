using System;
using Cinemachine;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Factories;
using GameCore.Source.Controllers.Api.Providers;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Controllers.Core.Presenters.GameLoop;
using GameCore.Source.Domain.EntityComponents;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using GameCore.Source.Presentation.Api.GameLoop;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameCore.Source.Controllers.Core.Factories
{
    public class PlayerFactory
    {
        private readonly HeroModel _heroModel;
        private readonly IPropertyProvider _propertyProvider;
        private readonly IAbilityUpgradeService _abilityUpgradeService;
        private readonly AbilityFactory _abilityFactory;
        private readonly IAudioPlayerService _audioPlayerService;
        private readonly PlayerModel _playerModel;
        private readonly IHealthViewBuilder _healthViewBuilder;

        private GameObject _playerPrefab;

        public PlayerFactory(
            HeroModel heroModel,
            IPropertyProvider propertyProvider,
            IAbilityUpgradeService abilityUpgradeService,
            AbilityFactory abilityFactory,
            IAudioPlayerService audioPlayerService,
            PlayerModel playerModel,
            IHealthViewBuilder healthViewBuilder)
        {
            _heroModel = heroModel ?? throw new ArgumentNullException(nameof(heroModel));
            _propertyProvider = propertyProvider ?? throw new ArgumentNullException(nameof(propertyProvider));
            _abilityUpgradeService =
                abilityUpgradeService ?? throw new ArgumentNullException(nameof(abilityUpgradeService));
            _abilityFactory = abilityFactory ?? throw new ArgumentNullException(nameof(abilityFactory));
            _audioPlayerService = audioPlayerService ?? throw new ArgumentNullException(nameof(audioPlayerService));
            _playerModel = playerModel ?? throw new ArgumentNullException(nameof(playerModel));
            _healthViewBuilder = healthViewBuilder ?? throw new ArgumentNullException(nameof(healthViewBuilder));
        }

        public PlayerController Create(IGameLoopService gameLoopService)
        {
            PlayerController playerController = Object.Instantiate(
                    _heroModel.CurrentSelectedHero.Prefab,
                    Vector3.zero,
                    Quaternion.identity)
                .GetComponent<PlayerController>();

            InputController inputController = playerController.GetComponent<InputController>();
            MoveController moveController = playerController.GetComponent<MoveController>();
            AbilityHandler abilityHandler = playerController.GetComponent<AbilityHandler>();
            IDamageable damageable = playerController.GetComponent<IDamageable>();
            IHeroView heroView = playerController.GetComponent<IHeroView>();

            inputController.Initialize();

            _playerModel.AbilityContainer = abilityHandler;
            _playerModel.IsFreeSlotAvailable = playerController.IsFreeSlotAvailable;
            _playerModel.Transform = playerController.transform;
            _playerModel.MoveController = moveController;

            HeroPresenter heroPresenter = new(
                heroView,
                playerController,
                damageable,
                moveController,
                abilityHandler,
                _heroModel,
                gameLoopService,
                _propertyProvider,
                _abilityFactory,
                _audioPlayerService);

            _abilityUpgradeService.BindToPlayer(_playerModel);

            Object.FindObjectOfType<CinemachineVirtualCamera>().Follow = playerController.transform;
            _playerModel.Camera = Object.FindObjectOfType<Camera>();

            heroView.Construct(heroPresenter);

            _healthViewBuilder.Build(playerController.gameObject);

            return playerController;
        }
    }
}