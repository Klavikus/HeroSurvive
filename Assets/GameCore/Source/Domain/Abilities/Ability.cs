using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;

namespace GameCore.Source.Domain.Abilities
{
    public class Ability
    {
        public void ResetUpgrades()
        {
            throw new System.NotImplementedException();
        }

        public bool CheckConfig(AbilityConfigSO abilityConfigSo)
        {
            throw new System.NotImplementedException();
        }

        public bool CanUpgrade { get; set; }
        public AbilityUpgradeData AvailableUpgrade { get; set; }
    }
}