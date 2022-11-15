using System;
using CodeBase.Domain.Enums;
using UnityEngine;

[Serializable]
public class AbilityUpgradeData
{
    [field: SerializeField] public AbilityUpgradeViewData ViewData { get; private set; }
    [field: SerializeField] public BaseProperty PropertyType { get; private set; }
    [field: SerializeField] public float Value { get; private set; }
}