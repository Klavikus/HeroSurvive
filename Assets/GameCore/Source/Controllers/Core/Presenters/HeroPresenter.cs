using System;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Factories;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.EntityComponents;
using GameCore.Source.Domain.Enums;
using GameCore.Source.Domain.Models;
using GameCore.Source.Presentation.Api;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class HeroPresenter : IPresenter
    {
        private readonly PlayerController _playerController;
        private readonly IHeroView _view;
        private readonly IDamageable _damageable;
        private readonly MoveController _moveController;
        private readonly AbilityHandler _abilityHandler;

        private readonly HeroModel _heroModel;
        private readonly IGameLoopService _gameLoopService;
        private readonly IPropertyProvider _propertyProvider;
        private readonly AbilityFactory _abilityFactory;
        private readonly IAudioPlayerService _audioPlayerService;

        public HeroPresenter(
            IHeroView view,
            PlayerController playerController,
            IDamageable damageable,
            MoveController moveController,
            AbilityHandler abilityHandler,
            HeroModel heroModel,
            IGameLoopService gameLoopService,
            IPropertyProvider propertyProvider,
            AbilityFactory abilityFactory,
            IAudioPlayerService audioPlayerService)
        {
            _playerController = playerController
                ? playerController
                : throw new ArgumentNullException(nameof(playerController));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _damageable = damageable ?? throw new ArgumentNullException(nameof(damageable));
            _moveController = moveController ? moveController : throw new ArgumentNullException(nameof(moveController));
            _abilityHandler = abilityHandler ? abilityHandler : throw new ArgumentNullException(nameof(abilityHandler));
            _heroModel = heroModel ?? throw new ArgumentNullException(nameof(heroModel));
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            _propertyProvider = propertyProvider ?? throw new ArgumentNullException(nameof(propertyProvider));
            _abilityFactory = abilityFactory ?? throw new ArgumentNullException(nameof(abilityFactory));
            _audioPlayerService = audioPlayerService ?? throw new ArgumentNullException(nameof(audioPlayerService));
        }

        public void Enable()
        {
            _abilityHandler.Initialize(_abilityFactory, _audioPlayerService);
            _abilityHandler.AddAbility(_heroModel.CurrentSelectedHero.InitialAbilityConfig);

            ApplyProperties();

            _propertyProvider.PropertiesUpdated += ApplyProperties;
            _damageable.Died += OnDied;
        }

        public void Disable()
        {
            _propertyProvider.PropertiesUpdated -= ApplyProperties;
            _damageable.Died -= OnDied;
        }

        private void ApplyProperties()
        {
            MainProperties currentProperties = _propertyProvider.GetResultProperties();

            _damageable.Initialize(new DamageableData(currentProperties));
            _moveController.Initialize(currentProperties.BaseProperties[BaseProperty.MoveSpeed]);
            _abilityHandler.UpdatePlayerModifiers(currentProperties.BaseProperties);
        }

        private void OnDied()
        {
            _gameLoopService.NotifyPlayerDeath();
        }
    }
}