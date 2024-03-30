using System;
using GameCore.Source.Domain.Enums;
using GameCore.Source.Domain.Models;
using Modules.Common.Utils;
using UnityEngine;

namespace GameCore.Source.Domain.Data
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