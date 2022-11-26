using CodeBase.Configs;
using CodeBase.Domain;
using CodeBase.Domain.Abilities;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enemies;
using CodeBase.Domain.Enums;
using CodeBase.ForSort;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services.PropertiesProviders;
using CodeBase.Infrastructure.StateMachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Damageable _damageable;
    [SerializeField] private MoveController _moveController;
    [SerializeField] private DamageableData _damageableData;
    [SerializeField] private AbilityHandler _abilityHandler;

    private IPropertyProvider _propertyProvider;
    private MainProperties _currentProperties;
    private LevelUpModel _levelUpModel;
    private AudioPlayerService _audioPlayerService;

    public bool IsFreeSlotAvailable => _abilityHandler.IsFreeSlotAvailable;
    public AbilityHandler AbilityHandler => _abilityHandler;

    public void Initialize(IPropertyProvider propertyProvider, AbilityConfigSO initialAbilityConfigSO,
        LevelUpModel levelUpModel,
        AbilityFactory abilityFactory, AudioPlayerService audioPlayerService)
    {
        _propertyProvider = propertyProvider;
        _levelUpModel = levelUpModel;
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