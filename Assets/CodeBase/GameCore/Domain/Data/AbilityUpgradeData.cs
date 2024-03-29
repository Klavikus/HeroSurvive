using System;
using CodeBase.GameCore.Configs;
using CodeBase.GameCore.Domain.Enums;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Data
{
    [Serializable]
    public class AbilityUpgradeData
    {
        [field: SerializeField] public BaseProperty PropertyType { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
        [field: SerializeField] public AbilityConfigSO BaseConfigSO { get; private set; }
        [field: SerializeField] public bool IsFirstAbilityGain { get; private set; }

        public void SetAbilityGainedStatus(bool gained) =>
            IsFirstAbilityGain = gained;
    }
}