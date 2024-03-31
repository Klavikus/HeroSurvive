using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Factories;
using GameCore.Source.Domain.Abilities;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.EntityComponents;
using GameCore.Source.Domain.Enums;
using GameCore.Source.Domain.Models;
using UnityEngine;

namespace GameCore.Source.Controllers.Core
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Damageable _damageable;
        [SerializeField] private MoveController _moveController;
        [SerializeField] private AbilityHandler _abilityHandler;

        private IPropertyProvider _propertyProvider;
        private MainProperties _currentProperties;
        private IAudioPlayerService _audioPlayerService;

        public bool IsFreeSlotAvailable => _abilityHandler.IsFreeSlotAvailable;
        public AbilityHandler AbilityHandler => _abilityHandler;
        public AbilityContainer AbilityContainer { get; set; }

        public void Initialize(
            IPropertyProvider propertyProvider,
            AbilityConfigSO initialAbilityConfigSO,
            AbilityFactory abilityFactory,
            IAudioPlayerService audioPlayerService,
            IGameLoopService gameLoopService)
        {
            _propertyProvider = propertyProvider;
            _audioPlayerService = audioPlayerService;
            _abilityHandler.Initialize(abilityFactory, _audioPlayerService, gameLoopService);
            _abilityHandler.AddAbility(initialAbilityConfigSO);
            _propertyProvider.PropertiesUpdated += OnPropertiesUpdated;

            OnPropertiesUpdated();
        }

        private void OnDisable()
        {
            if (_propertyProvider == null)
                return;

            _propertyProvider.PropertiesUpdated -= OnPropertiesUpdated;
        }

        private void OnPropertiesUpdated()
        {
            _currentProperties = _propertyProvider.GetResultProperties();

            _damageable.Initialize(new DamageableData(_currentProperties));
            _moveController.Initialize(_currentProperties.BaseProperties[BaseProperty.MoveSpeed]);
            _abilityHandler.UpdatePlayerModifiers(_currentProperties.BaseProperties);
            _abilityHandler.UpdatePlayerModifiers(_currentProperties.BaseProperties);
        }
    }
}