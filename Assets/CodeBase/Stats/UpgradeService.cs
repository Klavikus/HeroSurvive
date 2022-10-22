using CodeBase.Abilities;

namespace CodeBase.Stats
{
    public class UpgradeService
    {
        private PlayerStats _playerStats;

        public UpgradeService(PlayerStats playerStats)
        {
            _playerStats = playerStats;
        }

        // public void UpdateAbilityData(AbilityData abilityData)
        // {
        //     abilityData.UseModifiers(_playerStats);
        // }
    }
}