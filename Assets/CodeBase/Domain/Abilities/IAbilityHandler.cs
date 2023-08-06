using System.Collections.Generic;
using CodeBase.Configs;
using CodeBase.Infrastructure;

namespace CodeBase.Domain
{
    public interface IAbilityHandler
    {
        void Initialize(AbilityFactory abilityFactory, IAudioPlayerService audioPlayerService);
        void AddAbility(AbilityConfigSO newAbility);
        void UpdatePlayerModifiers(IReadOnlyDictionary<BaseProperty, float> stats);
    }
}