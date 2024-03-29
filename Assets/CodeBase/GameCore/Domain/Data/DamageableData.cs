using System;
using CodeBase.GameCore.Configs;
using CodeBase.GameCore.Domain.Enums;
using CodeBase.GameCore.Domain.Models;
using CodeBase.Utilities.Extensions;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Data
{
    [Serializable]
    public struct DamageableData
    {
        public DamageableData(MainProperties mainProperties)
        {
            MaxHealth = (int) mainProperties.BaseProperties[BaseProperty.MaxHealth];
            HealthRegenerationPercent = mainProperties.BaseProperties[BaseProperty.HealthRegen].AsPercentFactor();
            RegenerationDelayInSeconds = GameConstants.RegenerationDelay;
        }

        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public float HealthRegenerationPercent { get; private set; }
        [field: SerializeField] public float RegenerationDelayInSeconds { get; private set; }

        public void UpdateProgression(float stageProgressionModifier)
        {
            MaxHealth = (int) (MaxHealth * stageProgressionModifier);
            HealthRegenerationPercent *= stageProgressionModifier;
        }
    }
}