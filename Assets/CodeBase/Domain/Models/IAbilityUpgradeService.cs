using System;
using CodeBase.Domain.Data;
using CodeBase.Domain.EntityComponents;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Domain.Models
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