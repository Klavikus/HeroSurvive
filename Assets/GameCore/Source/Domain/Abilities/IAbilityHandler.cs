using System.Collections.Generic;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Enums;

namespace GameCore.Source.Domain.Abilities
{
    public interface IAbilityHandler
    {
        void AddAbility(AbilityConfigSO newAbility);
        void UpdatePlayerModifiers(IReadOnlyDictionary<BaseProperty, float> stats);
    }
}