using CodeBase.Configs;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Domain
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Damageable _damageable;
        [SerializeField] private MoveController _moveController;
        [SerializeField] private AbilityHandler _abilityHandler;

        private IPropertyProvider _propertyProvider;
        private MainProperties _currentProperties;
        private IAudioPlayerService _audioPlayerService;

        public bool IsFreeSlotAvailable => _abilityHandler.IsFreeSlotAvailable;
        public AbilityHandler AbilityHandler => _abilityHandler;

        public void Initialize(IPropertyProvider propertyProvider, AbilityConfigSO initialAbilityConfigSO,
            AbilityFactory abilityFactory, IAudioPlayerService audioPlayerService)
        {
            _propertyProvider = propertyProvider;
            _audioPlayerService = audioPlayerService;
            _abilityHandler.Initialize(abilityFactory, _audioPlayerService);
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