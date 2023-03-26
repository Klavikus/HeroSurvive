using System;
using CodeBase.Configs;
using UnityEngine;

namespace CodeBase.Domain
{
    [Serializable]
    public class AbilityUpgradeData
    {
        [field: SerializeField] public BaseProperty PropertyType { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
        [field: SerializeField] public AbilityConfigSO BaseConfigSO { get; private set; }

        [field: SerializeField]  public bool IsFirstAbilityGain { get; private set; }

        public void SetAbilityGainedStatus(bool gained) => IsFirstAbilityGain = gained;
    }
}