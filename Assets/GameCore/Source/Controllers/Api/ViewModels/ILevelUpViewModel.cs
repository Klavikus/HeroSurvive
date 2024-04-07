using System;
using GameCore.Source.Domain.Data;

namespace GameCore.Source.Controllers.Api.ViewModels
{
    public interface ILevelUpViewModel
    {
        event Action Rerolled;
        event Action<int> LevelChanged;
        event Action<AbilityUpgradeData[]> AvailableUpgradesChanged;
        event Action<float> LevelProgressChanged;
        void SelectUpgrade(AbilityUpgradeData abilityUpgradeData);
        void ResetLevels();
        AbilityUpgradeData[] GetAvailableUpgrades();
        void Reroll();
    }
}