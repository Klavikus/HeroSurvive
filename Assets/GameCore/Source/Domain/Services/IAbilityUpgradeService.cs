using System;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;

namespace GameCore.Source.Domain.Services
{
    public interface IAbilityUpgradeService
    {
        event Action<AbilityUpgradeData[]> AvailableUpgradesChanged;
        void UseUpgrade(AbilityUpgradeData abilityUpgradeData);
        void ResetUpgrades();
        void CalculateAvailableUpgrades();
        void BindToPlayer(PlayerModel player);
        AbilityUpgradeData[] GetAvailableUpgrades();
    }
}