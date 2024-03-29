using System;
using CodeBase.GameCore.Domain.Data;
using CodeBase.GameCore.Domain.EntityComponents;
using CodeBase.GameCore.Infrastructure.Services;

namespace CodeBase.GameCore.Domain.Services
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