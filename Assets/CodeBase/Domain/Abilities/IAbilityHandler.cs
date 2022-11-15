using System.Collections.Generic;
using CodeBase.Domain.Enums;

namespace CodeBase.Domain.Abilities
{
    public interface IAbilityHandler
    {
        void Initialize();
        void AddAbility(Ability newAbility);
        void UpdateAbilityData(IReadOnlyDictionary<BaseProperty, float> stats);
    }
}