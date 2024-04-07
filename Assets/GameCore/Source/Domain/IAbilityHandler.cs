using System.Collections.Generic;
using GameCore.Source.Domain.Abilities;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Enums;

namespace GameCore.Source.Domain
{
    public interface IAbilityHandler
    {
        // IReadOnlyList<Ability> CurrentAbilities { get; }
        void AddAbility(AbilityConfigSO newAbility);
        void UpdatePlayerModifiers(IReadOnlyDictionary<BaseProperty, float> stats);
        void UpgradeAbility(AbilityUpgradeData abilityUpgradeData);
        IReadOnlyList<IAbilityController> CurrentAbilities { get; }
    }
}