using System;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Enums;
using UnityEngine;

namespace GameCore.Source.Domain.Data
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