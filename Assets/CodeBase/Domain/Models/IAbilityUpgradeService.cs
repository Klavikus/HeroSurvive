using System;
using CodeBase.Domain;
using CodeBase.Infrastructure;

namespace CodeBase.Domain
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