using System.Collections.Generic;
using GameCore.Source.Domain.Data;

namespace GameCore.Source.Domain.Abilities
{
    public class AbilityContainer
    {
        public void UpgradeAbility(AbilityUpgradeData abilityUpgradeData)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyList<Ability> CurrentAbilities { get; set; }
    }
}