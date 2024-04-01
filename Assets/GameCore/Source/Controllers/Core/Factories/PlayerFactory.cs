using Cinemachine;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Domain.EntityComponents;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using GameCore.Source.Presentation.Api;
using UnityEngine;

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

        private GameObject _playerPrefab;

        public PlayerFactory(
            HeroModel heroModel,
            IPropertyProvider propertyProvider,
            IAbilityUpgradeService abilityUpgradeService,
            AbilityFactory abilityFactory,
            IAudioPlayerService audioPlayerService,
            PlayerModel playerModel)
        {
            _heroModel = heroModel;
            _propertyProvider = propertyProvider;
            _abilityUpgradeService = abilityUpgradeService;
            _abilityFactory = abilityFactory;
            _audioPlayerService = audioPlayerService;
            _playerModel = playerModel;
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

            _playerModel.AbilityContainer = playerController.AbilityContainer;
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

            return playerController;
        }
    }
}