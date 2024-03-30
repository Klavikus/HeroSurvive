using System;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.EntityComponents;

namespace GameCore.Source.Domain.Services
{
    public interface IAbilityUpgradeService : IService
    {
        event Action<AbilityUpgradeData[]> AvailableUpgradesChanged;
        void UseUpgrade(AbilityUpgradeData abilityUpgradeData);
        void ResetUpgrades();
        void CalculateAvailableUpgrades();
        void BindToPlayer(Player player);
        AbilityUpgradeData[] GetAvailableUpgrades();
    }
}