using System.Collections.Generic;
using CodeBase.Configs;
using CodeBase.Domain.Enums;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Domain.Abilities
{
    public interface IAbilityHandler
    {
        void Initialize(AbilityFactory abilityFactory, IAudioPlayerService audioPlayerService);
        void AddAbility(AbilityConfigSO newAbility);
        void UpdatePlayerModifiers(IReadOnlyDictionary<BaseProperty, float> stats);
    }
}