using System;
using CodeBase.Domain;
using CodeBase.Domain.Abilities;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enemies;
using CodeBase.Domain.Enums;
using CodeBase.ForSort;
using CodeBase.Infrastructure.Services.PropertiesProviders;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Damageable _damageable;
    [SerializeField] private MoveController _moveController;
    [SerializeField] private DamageableData _damageableData;
    [SerializeField] private AbilityHandler _abilityHandler;

    private IPropertyProvider _propertyProvider;
    private MainProperties _currentProperties;

    public void Initialize(IPropertyProvider propertyProvider, Ability initialAbility)
    {
        _propertyProvider = propertyProvider;
        _abilityHandler.AddAbility(initialAbility);
        _abilityHandler.Initialize();
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
        _abilityHandler.UpdateAbilityData(_currentProperties.BaseProperties);
    }
}