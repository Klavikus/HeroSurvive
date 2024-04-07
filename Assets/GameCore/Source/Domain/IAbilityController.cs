using System.Collections.Generic;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Enums;

namespace GameCore.Source.Domain
{
    public interface IAbilityController
    {
        bool CanUpgrade { get; }
        AbilityUpgradeData AvailableUpgrade { get; }
        void Execute();
        void UpdatePlayerModifiers(IReadOnlyDictionary<BaseProperty, float> stats);
        void Upgrade();
        void ResetUpgrades();
        bool CheckConfig(AbilityConfigSO abilityConfigSO);
        void Dispose();
    }
}