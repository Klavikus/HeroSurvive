using System;
using CodeBase.Domain.Enums;
using UnityEngine;

namespace CodeBase.Domain.Data
{
    [Serializable]
    public struct DamageableData
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public float HealthRegenerationPercent { get; private set; }
        [field: SerializeField] public float RegenerationDelayInSeconds { get; private set; }

        public DamageableData(MainProperties mainProperties)
        {
            MaxHealth = (int) mainProperties.BaseProperties[BaseProperty.MaxHealth];
            HealthRegenerationPercent = mainProperties.BaseProperties[BaseProperty.HealthRegen];
            RegenerationDelayInSeconds = 1f;
        }

        public void UpdateProgression(float stageProgressionModifier)
        {
            MaxHealth = (int) (MaxHealth * stageProgressionModifier);
            HealthRegenerationPercent *= stageProgressionModifier;
        }
    }
}